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

        public VotesController(VoteService voteService)
        {
            _voteService = voteService;
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

        [HttpPost]
        public async Task<IActionResult> PostVote([FromBody]string payload)
        {
            var voteDto = await new EncryptionConfig<VoteDto>().DecryptPayload(payload);

            var insertResult = await _voteService.InsertVote(voteDto);

            if (insertResult == null)
                return BadRequest();

            return Ok(insertResult);
        }

        //[AllowAnonymous]
        [Route("[action]/{electionId}")]
        [HttpGet]
        public async Task<IActionResult> CountVotes(string electionId)
        {
            var countedVotes = await _voteService.CountVotes(electionId);

            if(countedVotes == null || countedVotes.Count == 0)
            {
                return NotFound(new { Error = "No votes" });
            }

            return Ok(countedVotes);
        }
    }
}
