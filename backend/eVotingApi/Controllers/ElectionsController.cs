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
    [Route("api/Elections")]
    [ApiController]
    public class ElectionsController : ControllerBase
    {
        private readonly eVotingContext _context;

        public ElectionsController(eVotingContext context)
        {
            _context = context;
        }

        // GET: api/Elections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ElectionDTO>>> GetElections()
        {
            return await _context.Elections
                .Select(x => ElectionToDTO(x))
                .ToListAsync();
        }

        // GET: api/Elections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ElectionDTO>> GetElection(long id)
        {
            var election = await _context.Elections.FindAsync(id);

            if (election == null)
            {
                return NotFound();
            }

            return ElectionToDTO(election);
        }

        // PUT: api/Elections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutElection(long id, ElectionDTO electionDTO)
        {
            if (id != electionDTO.ElectionId)
            {
                return BadRequest();
            }

            var election = await _context.Elections.FindAsync(id);

            if(election == null)
            {
                return NotFound();
            }

            election.ElectionType = electionDTO.ElectionType;
            election.ElectionDate = electionDTO.ElectionDate;

            _context.Entry(election).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ElectionExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Elections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Election>> PostElection(ElectionDTO electionDTO)
        {
            var election = new Election
            {
                ElectionType = electionDTO.ElectionType,
                ElectionDate = electionDTO.ElectionDate
            };

            _context.Elections.Add(election);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetElection", new { id = election.ElectionId }, ElectionToDTO(election));
        }

        // DELETE: api/Elections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteElection(long id)
        {
            var election = await _context.Elections.FindAsync(id);
            if (election == null)
            {
                return NotFound();
            }

            _context.Elections.Remove(election);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ElectionExists(long id)
        {
            return _context.Elections.Any(e => e.ElectionId == id);
        }

        private static ElectionDTO ElectionToDTO(Election election) =>
            new ElectionDTO
            {
                ElectionId = election.ElectionId,
                ElectionType = election.ElectionType,
                ElectionDate = election.ElectionDate
            };
    }
}
