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

namespace eVotingApi.Controllers
{
    [Authorize]
    [Route("api/Voters")]
    [ApiController]
    public class VotersController : ControllerBase
    {
        //private readonly eVotingContext _context;
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

        [Route("[action]/{constituencyName}")]
        [HttpGet]
        public async Task<IActionResult> GetByConstituency(string constituencyName)
        {
            var constituency = await _voterService.GetByConstituency(constituencyName);

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

        /*// GET: api/Voters
        [HttpGet]
        public ActionResult<Task<IEnumerable<Voter>>> GetVoters()
        {
            return _voterService.Get();
        }

        // GET: api/Voters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Voter>> GetVoter(long id)
        {
            var voter = await _context.Voters.FindAsync(id);

            if (voter == null)
            {
                return NotFound();
            }

            return voter;
        }

        [Route("[action]/{constituencyId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voter>>> GetByConstituencyId(long constituencyId)
        {
            return await _context.Voters
                .Where(x => x.ConstituencyId == constituencyId)
                .ToListAsync();
        }

        [Route("[action]/{votersId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SecurityQuestionsDTO>>> GetSecurityQuestionsById(long votersId)
        {
            var questions = await _context.Voters.Where(x => x.VoterId == votersId).Select(x => QuestionsToDTO(x)).ToListAsync();

            if(questions == null)
            {
                return NotFound();
            }

            return questions;
        }

        // PUT: api/Voters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoter(long id, Voter voter)
        {
            if (id != voter.VoterId)
            {
                return BadRequest();
            }

            _context.Entry(voter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Voters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Voter>> PostVoter(Voter voter)
        {
            _context.Voters.Add(voter);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVoter", new { id = voter.VoterId }, voter);
        }

        // DELETE: api/Voters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoter(long id)
        {
            var voter = await _context.Voters.FindAsync(id);
            if (voter == null)
            {
                return NotFound();
            }

            _context.Voters.Remove(voter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VoterExists(long id)
        {
            return _context.Voters.Any(e => e.VoterId == id);
        }*/
    }
}
