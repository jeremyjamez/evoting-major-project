using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eVotingApi.Models;
using eVotingApi.Data;
using eVotingApi.Services;
using Microsoft.AspNetCore.Authorization;
using eVotingApi.Models.DTO;
using eVotingApi.Config;
using System.Text.Json;

namespace eVotingApi.Controllers
{
    [Authorize]
    [Route("api/Candidates")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly CandidateService _candidateService;

        public CandidatesController(CandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        // GET: api/Candidates/1234567
        [HttpGet("{voterId}")]
        public async Task<IActionResult> GetCandidates(string voterId)
        {
            var candidates = await _candidateService.GetCandidates(voterId);
            //var serializedObj = JsonSerializer.Serialize(candidates.ToArray(), candidates.ToArray().GetType());
            //var encryptedResponse = await new EncryptionConfig<List<CandidateDTO>>().EncryptPayload(serializedObj, voterId);
            return Ok(candidates);
        }

        /*// GET: api/Candidates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidates()
        {
            return await _context.Candidates
                .Include(c => c.Constituency)
                .Include(c => c.Member)
                .ThenInclude(m => m.PoliticalParty)
                .Include(c => c.Election)
                .ToListAsync();
        }

        // GET: api/Candidates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Candidate>> GetCandidate(long id)
        {
            var candidate = await _context.Candidates.FindAsync(id);

            if (candidate == null)
            {
                return NotFound();
            }

            return candidate;
        }

        // GET: api/Candidates/5
        [Route("[action]/{constituencyId}")]
        [HttpGet]
        public async Task<ActionResult<Candidate>> GetByConstituency(long constituencyId)
        {
            var candidate = await _context.Candidates.Where(c => c.ConstituencyId == constituencyId).FirstAsync();

            if (candidate == null)
            {
                return NotFound();
            }

            return candidate;
        }

        // PUT: api/Candidates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCandidate(long id, Candidate candidate)
        {
            if (id != candidate.CandidateId)
            {
                return BadRequest();
            }

            _context.Entry(candidate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when(!CandidateExists(id))
            {
                if (!CandidateExists(id))
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

        // POST: api/Candidates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Candidate>> PostCandidate(Candidate candidate)
        {
            _context.Candidates.Add(candidate);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CandidateExists(candidate.CandidateId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCandidate", new { id = candidate.CandidateId }, candidate);
        }

        // DELETE: api/Candidates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(long id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }

            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CandidateExists(long id)
        {
            return _context.Candidates.Any(e => e.CandidateId == id);
        }*/
    }
}
