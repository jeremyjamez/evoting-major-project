using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eVotingApi.Models;
using eVotingApi.Data;
using Microsoft.AspNetCore.Authorization;
using eVotingApi.Services;
using Google.Authenticator;
using eVotingApi.Config;
using System.Security.Cryptography;
using System.Text;
using eVotingApi.Models.DTO;
using System.Text.Json;

namespace eVotingApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly VoteService _voteService;
        private readonly VoterService _voterService;

        public VotesController(VoteService voteService, VoterService voterService)
        {
            _voteService = voteService;
            _voterService = voterService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVote(string id)
        {
            var vote = await _voteService.GetVote(id);

            if (vote == null)
                return NotFound();

            //var serializedObj = JsonSerializer.Serialize(vote, vote.GetType());
            //var encryptedVote = await new EncryptionConfig<VoteDto>().EncryptPayload(serializedObj, vote.VoterId);
            return Ok(vote);
        }

        [HttpPost("{pin}")]
        public async Task<IActionResult> PostVote(string pin, [FromBody]string payload)
        {
            var tfa = new TwoFactorAuthenticator();

            var vote = await new EncryptionConfig<Vote>().DecryptPayload(payload);

            string salt = await _voterService.GetSecretCodeSalt(vote.VoterId);

            var hashedCode = ComputeHash(Encoding.UTF8.GetBytes(vote.VoterId), Encoding.UTF8.GetBytes(salt));

            if(tfa.ValidateTwoFactorPIN(hashedCode, pin))
            {
                var insertResult = await _voteService.InsertVote(vote);

                if (insertResult == string.Empty)
                    return BadRequest();

                return Ok(insertResult);
            }
            else
            {
                return BadRequest(new { IsCorrect = false });
            }
        }

        private string ComputeHash(byte[] bytesToHash, byte[] salt)
        {
            var byteResult = new Rfc2898DeriveBytes(bytesToHash, salt, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(8));
        }
    }
}
