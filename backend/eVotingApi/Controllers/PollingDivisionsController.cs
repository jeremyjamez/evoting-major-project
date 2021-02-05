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
    [Route("api/[controller]")]
    [ApiController]
    public class PollingDivisionsController : ControllerBase
    {
        private readonly eVotingContext _context;

        public PollingDivisionsController(eVotingContext context)
        {
            _context = context;
        }

        // GET: api/PollingDivisions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollingDivision>>> GetPollingDivisions()
        {
            return await _context.PollingDivisions.ToListAsync();
        }

        // GET: api/PollingDivisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PollingDivision>> GetPollingDivision(long id)
        {
            var pollingDivision = await _context.PollingDivisions.FindAsync(id);

            if (pollingDivision == null)
            {
                return NotFound();
            }

            return pollingDivision;
        }

        // PUT: api/PollingDivisions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPollingDivision(long id, PollingDivision pollingDivision)
        {
            if (id != pollingDivision.DivisionId)
            {
                return BadRequest();
            }

            _context.Entry(pollingDivision).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollingDivisionExists(id))
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

        // POST: api/PollingDivisions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PollingDivision>> PostPollingDivision(PollingDivision pollingDivision)
        {
            _context.PollingDivisions.Add(pollingDivision);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPollingDivision", new { id = pollingDivision.DivisionId }, pollingDivision);
        }

        // DELETE: api/PollingDivisions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePollingDivision(long id)
        {
            var pollingDivision = await _context.PollingDivisions.FindAsync(id);
            if (pollingDivision == null)
            {
                return NotFound();
            }

            _context.PollingDivisions.Remove(pollingDivision);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PollingDivisionExists(long id)
        {
            return _context.PollingDivisions.Any(e => e.DivisionId == id);
        }
    }
}
