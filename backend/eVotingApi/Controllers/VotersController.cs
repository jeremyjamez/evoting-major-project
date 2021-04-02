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

namespace eVotingApi.Controllers
{
    [Authorize(Roles = "EDW,ECJ")]
    [Route("api/Voters")]
    [ApiController]
    public class VotersController : ControllerBase
    {
        private readonly eVotingContext _context;

        public VotersController(eVotingContext context)
        {
            _context = context;
        }

        // GET: api/Voters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voter>>> GetVoters()
        {
            return await _context.Voters
                .Include(v => v.Constituency)
                .ToListAsync();
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
        }

        private static SecurityQuestionsDTO QuestionsToDTO(Voter voter) =>
            new SecurityQuestionsDTO
            {
                Address = voter.Address,
                DateOfBirth = voter.DateOfBirth,
                Telephone = voter.Telephone,
                Occupation = voter.Occupation,
                MothersMaidenName = voter.MothersMaidenName
            };
    }
}
