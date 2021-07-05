using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eVotingApi.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using eVotingApi.Services;
using System.Security.Cryptography;
using Google.Authenticator;
using System.Text;
using eVotingApi.Models.DTO.Responses;
using System.IdentityModel.Tokens.Jwt;
using eVotingApi.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.IO;

namespace eVotingApi.Controllers
{
    [Authorize]
    [Route("api/Voters")]
    [ApiController]
    public class VotersController : ControllerBase
    {
        private readonly VoterService _voterService;
        private readonly JwtConfig _jwtConfig;
        private static Random _rng;

        public VotersController(VoterService voterService, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _voterService = voterService;
            _jwtConfig = optionsMonitor.CurrentValue;
            _rng = new Random();
        }

        // GET: api/Voters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoterDto>>> GetVoters()
        {
            return Ok(await _voterService.Get());
        }

        // GET: api/Voters/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoter(string id)
        {
            var voter = await _voterService.GetById(id);

            if (voter == null)
            {
                return NotFound();
            }

            return Ok(voter);
        }

        [Route("[action]/{constituencyId}")]
        [HttpGet]
        public async Task<IActionResult> GetByConstituencyId(string constituencyId)
        {
            var constituency = await _voterService.GetByConstituencyId(constituencyId);

            if(constituency == null)
            {
                return NotFound();
            }

            return Ok(constituency);
        }

        [AllowAnonymous]
        [Route("[action]")]
        [ActionName("IsRegistered")]
        [HttpPost]
        public async Task<ActionResult<RegisteredResponse>> IsVoterRegistered([FromBody]VoterDto voterDto)
        {
            return await _voterService.IsRegistered(voterDto);
        }

        [AllowAnonymous]
        [Route("[action]")]
        [ActionName("Validate")]
        [HttpPost]
        public async Task<ActionResult<ValidationResponse>> ValidatePin([FromBody]string payload)
        {
            var tfa = new TwoFactorAuthenticator();

            var pinData = await new EncryptionConfig<PinData>().DecryptPayload(payload);

            string salt = await _voterService.GetSecretCodeSalt(pinData.VoterId);

            var hashedCode = ComputeHash(Encoding.UTF8.GetBytes(pinData.VoterId), Encoding.UTF8.GetBytes(salt));

            var response = GenerateJwtToken(tfa.ValidateTwoFactorPIN(hashedCode, pinData.Pin), pinData.VoterId);
            return response;
        }

        [AllowAnonymous]
        [Route("[action]/{voterId}")]
        [HttpGet]
        public async Task<ActionResult<PairingInfo>> Pair(string voterId)
        {
            var newSalt = GenerateSalt();
            var manualCode = ComputeHash(Encoding.UTF8.GetBytes(voterId), Encoding.UTF8.GetBytes(newSalt));

            var result = await _voterService.UpdateHashedSecretCode(newSalt, voterId);

            if (result.ModifiedCount > 0)
            {
                var tfa = new TwoFactorAuthenticator();
                var setupInfo = tfa.GenerateSetupCode("Jamaica Online E-Voting Prototype", voterId.ToString(), manualCode, false, 3);
                var qr = setupInfo.QrCodeSetupImageUrl.Replace("http://", "https://");
                PairingInfo pi = new PairingInfo { ManualSetupCode = setupInfo.ManualEntryKey, QR = qr };
                return Ok(pi);
            }

            return BadRequest();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetQuestions([FromBody]string payload)
        {
            var votersId = await new EncryptionConfig<string>().DecryptPayload(payload);
            var questions = await _voterService.GetQuestions(votersId);

            if (questions == null)
            {
                return NotFound();
            }

            var q = new List<SecurityQuestion>() {
                new SecurityQuestion { Question = "Telephone Number", Answer = questions.Telephone },
                new SecurityQuestion { Question = "Occupation", Answer = questions.Occupation },
                new SecurityQuestion { Question = "Mother's maiden name", Answer = questions.MothersMaidenName },
                new SecurityQuestion { Question = "Parish of birth", Answer = questions.PlaceOfBirth },
                new SecurityQuestion { Question = "Mother's parish of birth", Answer = questions.MothersPlaceOfBirth }
            };

            List<SecurityQuestion> securityQuestions = SelectTwoQuestions(q);

            var serializedObj = JsonSerializer.Serialize(securityQuestions.ToArray(), securityQuestions.ToArray().GetType());

            var encryptedJson = await new EncryptionConfig<List<SecurityQuestion>>().EncryptPayload(serializedObj, votersId);

            return Ok(encryptedJson);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> VerifyVoter([FromBody] VerificationRequest payload)
        {
            //var decryptedData = await new EncryptionConfig<VerificationRequest>().DecryptPayload(payload);
            var bytes = Convert.FromBase64String(payload.Photo);

            var result = await _voterService.VerifyVoterIdentity(payload.VoterId, bytes);

            if (result == null)
                return BadRequest();

            return Ok(result);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetVoterElections()
        {
            var voterElections = await _voterService.GetVoter_Elections();

            if (voterElections == null)
                return NotFound();

            return Ok(voterElections);
        }

        /// <summary>
        /// Randomly selects two security questions from an array of the voter's information
        /// </summary>
        /// <param name="arr"></param>
        /// <returns>An object array containing two randomly selected questions</returns>
        [NonAction]
        private List<SecurityQuestion> SelectTwoQuestions(List<SecurityQuestion> arr)
        {
            int i = 0;
            List<SecurityQuestion> t = new List<SecurityQuestion>();

            for (int n = arr.Count; n > 1;)
            {
                int k = _rng.Next(n);

                if(!t.Contains(arr[k]))
                {
                    --n;
                    t.Add(arr[k]);

                    if (i == 1)
                        return t;

                    i++;
                }
            }

            return null;
        }

        /// <summary>
        /// Generates a JWT Token for the voter and returns a ValidationResponse based on the state of the PIN
        /// </summary>
        /// <param name="isCorrect"></param>
        /// <param name="voterId"></param>
        /// <returns>ValidationResponse</returns>
        [NonAction]
        private ValidationResponse GenerateJwtToken(bool isCorrect, string voterId)
        {
            if (!isCorrect)
                return new ValidationResponse() { IsCorrect = false };

            // Now its time to define the jwt token which will be responsible of creating our tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // We get our secret from the appsettings
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            // we define our token descriptor
            // We need to utilise claims which are properties in our token which gives information about the token
            // which belong to the specific user who it belongs to
            // so it could contain their id, name, email the good part is that these information
            // are generated by our server and identity framework which is valid and trusted
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", voterId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15d),
                // here we are adding the encryption alogorithim information which will be used to decrypt our token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return new ValidationResponse()
            {
                Token = jwtToken,
                IsCorrect = isCorrect
            };
        }

        [NonAction]
        private string GenerateSalt()
        {
            var bytes = new byte[8];
            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        [NonAction]
        private string ComputeHash(byte[] bytesToHash, byte[] salt)
        {
            var byteResult = new Rfc2898DeriveBytes(bytesToHash, salt, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(8));
        }

        #region Voter Specific POCO
        public class PairingInfo
        {
            public string ManualSetupCode { get; set; }
            public string QR { get; set; }
        }

        public class PinData
        {
            public string Pin { get; set; }
            public string VoterId { get; set; }
        }

        public class VerificationRequest
        {
            public string VoterId { get; set; }
            public string Photo { get; set; }
        }

        public class ValidationResponse
        {
            public string Token { get; set; }
            public bool IsCorrect { get; set; }
        }

        public class SecurityQuestion
        {
            public string Question { get; set; }
            public string Answer { get; set; }
        }
        #endregion
    }
}
