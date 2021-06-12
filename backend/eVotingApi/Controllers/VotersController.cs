using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eVotingApi.Models;
using eVotingApi.Models.DTO;
using eVotingApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Newtonsoft.Json;
using System.Diagnostics;

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

        [AllowAnonymous]
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetPublicKey()
        {
            return StatusCode(200, _voterService.GetPublicKey());
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
        public async Task<ActionResult<RegisteredResponse>> IsVoterRegistered([FromBody]string payload)
        {
            var cert = await GetCertificateAsync();
            var rsaCng = (RSACng)cert.PrivateKey;
            var decryptedMessageJson = Encoding.UTF8.GetString(rsaCng.Decrypt(Convert.FromBase64String(payload), RSAEncryptionPadding.OaepSHA1));
            VoterDto voterDto = JsonConvert.DeserializeObject<VoterDto>(decryptedMessageJson);
            return await _voterService.IsRegistered(voterDto);
        }

        [AllowAnonymous]
        [Route("[action]")]
        [ActionName("Validate")]
        [HttpPost]
        public async Task<ActionResult<ValidationResponse>> ValidatePin([FromBody]PinData pinData)
        {
            var tfa = new TwoFactorAuthenticator();

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

        [Route("[action]/{votersId}")]
        [HttpGet]
        public async Task<ActionResult<object[]>> GetQuestions(string votersId)
        {
            var questions = await _voterService.GetQuestions(votersId);

            if (questions == null)
            {
                return NotFound();
            }

            var q = new object[] {
                new { address = questions.Address },
                new { telephoneNumber = questions.Telephone },
                new { occupation = questions.Occupation },
                new { mothersMaidenName = questions.MothersMaidenName },
                new { placeOfBirth = questions.PlaceOfBirth },
                new { mothersPlaceOfBirth = questions.MothersPlaceOfBirth }
            };

            object[] temp = SelectTwoQuestions(q);

            return temp;
        }

        /// <summary>
        /// Randomly selects two security questions from an array of the voter's information
        /// </summary>
        /// <param name="arr"></param>
        /// <returns>An object array containing two randomly selected questions</returns>
        private object[] SelectTwoQuestions(object[] arr)
        {
            int i = 0;
            object[] t = new object[2];

            for (int n = arr.Length; n > 1;)
            {
                int k = _rng.Next(n);

                if(!t.Contains(arr[k]))
                {
                    --n;
                    t[i] = arr[k];

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
        private ValidationResponse GenerateJwtToken(bool isCorrect, string voterId)
        {
            if (!isCorrect)
                return new ValidationResponse() { IsCorrect = isCorrect };

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

        private string GenerateSalt()
        {
            var bytes = new byte[8];
            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string ComputeHash(byte[] bytesToHash, byte[] salt)
        {
            var byteResult = new Rfc2898DeriveBytes(bytesToHash, salt, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(8));
        }

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

        public class ValidationResponse
        {
            public string Token { get; set; }
            public bool IsCorrect { get; set; }
        }

        private async Task<X509Certificate2> GetCertificateAsync()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            var secret = await keyVaultClient.GetSecretAsync("https://evoting-keys.vault.azure.net/secrets/certificate-base64/d471eebb69b94aae967136e6b2d37b82").ConfigureAwait(false);
            var pfxBase64 = secret.Value;
            var bytes = Convert.FromBase64String(pfxBase64);
            var coll = new X509Certificate2Collection();
            coll.Import(bytes, "QEk5$s2PrRcZrj4xHb", X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            return coll[0];
        }
    }
}
