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

namespace eVotingApi.Controllers
{
    [Authorize(Roles = "EDW,EOJ")]
    [Route("api/Voters")]
    [ApiController]
    public class VotersController : ControllerBase
    {
        //private readonly eVotingContext _context;
        private readonly VoterService _voterService;

        public VotersController(VoterService voterService)
        {
            _voterService = voterService;
        }

        // GET: api/Voters
        [AllowAnonymous]
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
        [Route("[action]/{pin}/{voterId}")]
        [HttpGet]
        public async Task<ActionResult<bool>> ValidatePin(string pin, string voterId)
        {
            var tfa = new TwoFactorAuthenticator();

            string salt = await _voterService.GetSecretCodeSalt(voterId);

            var hashedCode = ComputeHash(Encoding.UTF8.GetBytes(voterId.ToString()), Encoding.UTF8.GetBytes(salt));

            bool isCorrect = tfa.ValidateTwoFactorPIN(hashedCode, pin);
            return isCorrect;
        }

        [AllowAnonymous]
        [Route("[action]/{voterId}")]
        [HttpGet]
        public async Task<ActionResult<PairingInfo>> Pair(long voterId)
        {
            var newSalt = GenerateSalt();
            var manualCode = ComputeHash(Encoding.UTF8.GetBytes(voterId.ToString()), Encoding.UTF8.GetBytes(newSalt));

            await _voterService.UpdateHashedSecretCode(newSalt, voterId);

            var tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode("Jamaica Online E-Voting Prototype", voterId.ToString(), manualCode, false, 3);
            var qr = setupInfo.QrCodeSetupImageUrl.Replace("http://", "https://");
            PairingInfo pi = new PairingInfo { ManualSetupCode = setupInfo.ManualEntryKey, QR = qr };
            return Ok(pi);
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
