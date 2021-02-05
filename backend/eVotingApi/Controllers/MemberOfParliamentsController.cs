using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eVotingApi.Models;
using eVotingApi.Data;

namespace eVotingApi.Controllers
{
    [Route("api/MemberOfParliaments")]
    [ApiController]
    public class MemberOfParliamentsController : ControllerBase
    {
        private readonly eVotingContext _context;

        public MemberOfParliamentsController(eVotingContext context)
        {
            _context = context;
        }

        // GET: api/MemberOfParliaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberOfParliament>>> GetMemberOfParliaments()
        {
            return await _context.MemberOfParliaments
                .Include(x => x.Candidate)
                .ThenInclude(c => c.Member)
                .ThenInclude(m => m.PoliticalParty)
                .ToListAsync();
        }

        // GET: api/MemberOfParliaments/5
        [HttpGet("{candidateId}/{constituencyId}")]
        public async Task<ActionResult<MemberOfParliament>> GetMemberOfParliament(long candidateId, long constituencyId)
        {
            var memberOfParliament = await _context.MemberOfParliaments.FindAsync(candidateId, constituencyId);

            if (memberOfParliament == null)
            {
                return NotFound();
            }

            return memberOfParliament;
        }

        // PUT: api/MemberOfParliaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMemberOfParliament(long id, MemberOfParliament memberOfParliament)
        {
            if (id != memberOfParliament.ConstituencyId)
            {
                return BadRequest();
            }

            _context.Entry(memberOfParliament).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberOfParliamentExists(id))
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

        // POST: api/MemberOfParliaments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MemberOfParliament>> PostMemberOfParliament(MemberOfParliament memberOfParliament)
        {
            _context.MemberOfParliaments.Add(memberOfParliament);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MemberOfParliamentExists(memberOfParliament.ConstituencyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMemberOfParliament", new { id = memberOfParliament.ConstituencyId }, memberOfParliament);
        }

        // DELETE: api/MemberOfParliaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemberOfParliament(long id)
        {
            var memberOfParliament = await _context.MemberOfParliaments.FindAsync(id);
            if (memberOfParliament == null)
            {
                return NotFound();
            }

            _context.MemberOfParliaments.Remove(memberOfParliament);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberOfParliamentExists(long id)
        {
            return _context.MemberOfParliaments.Any(e => e.ConstituencyId == id);
        }
    }
}
