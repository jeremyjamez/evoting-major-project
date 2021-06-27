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
using eVotingApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace eVotingApi.Controllers
{
    //[Authorize]
    [Route("api/PoliticalParties")]
    [ApiController]
    public class PoliticalPartiesController : ControllerBase
    {
        private readonly PartyService _partyService;

        public PoliticalPartiesController(PartyService partyService)
        {
            _partyService = partyService;
        }

        // GET: api/PoliticalParties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Party>>> GetPoliticalParties()
        {
            var parties = await _partyService.GetParties();

            if(parties == null)
            {
                return BadRequest(parties);
            }

            return Ok(parties);
        }

        // GET: api/PoliticalParties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Party>> GetPoliticalParty(string id)
        {
            var politicalParty = await _partyService.GetParty(id);

            if (politicalParty == null)
            {
                return NotFound();
            }

            return politicalParty;
        }
    }
}
