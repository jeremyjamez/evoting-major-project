﻿using System;
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

namespace eVotingApi.Controllers
{
    [Authorize]
    [Route("api/Elections")]
    [ApiController]
    public class ElectionsController : ControllerBase
    {
        private readonly ElectionService _electionService;

        public ElectionsController(ElectionService electionService)
        {
            _electionService = electionService;
        }

        // GET: api/Elections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Election>>> GetElections()
        {
            var elections = await _electionService.GetElections();

            if(elections == null)
            {
                return NotFound();
            }

            return Ok(elections);
        }

        [Route("[action]/{time}")]
        [HttpGet]
        public async Task<IActionResult> GetByTime(string time)
        {
            var electionId = await _electionService.GetByTime(long.Parse(time));

            if (electionId == null)
            {
                return NotFound();
            }

            return Ok(electionId);
        }

        // GET: api/Elections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Election>> GetElection(string id)
        {
            var election = await _electionService.GetElection(id);

            if (election == null)
            {
                return NotFound();
            }

            return Ok(election);
        }

        // POST: api/Elections
        [HttpPost]
        public async Task<ActionResult<object>> PostElection(Election election)
        {
            var electionId = await _electionService.AddElection(election);

            if (electionId == null)
                return BadRequest();

            return CreatedAtAction("GetElection", new { id = electionId }, election);
        }

        // DELETE: api/Elections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteElection(string id)
        {
            var deleteResult = await _electionService.Delete(id);

            if (deleteResult.DeletedCount < 1)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
