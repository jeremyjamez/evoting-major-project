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
    public class PollingCentresController : ControllerBase
    {
        private readonly eVotingContext _context;

        public PollingCentresController(eVotingContext context)
        {
            _context = context;
        }

        // GET: api/PollingCentres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollingCentre>>> GetPollingCentres()
        {
            return await _context.PollingCentres.ToListAsync();
        }

        // GET: api/PollingCentres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PollingCentre>> GetPollingCentre(long id)
        {
            var pollingCentre = await _context.PollingCentres.FindAsync(id);

            if (pollingCentre == null)
            {
                return NotFound();
            }

            return pollingCentre;
        }

        // PUT: api/PollingCentres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPollingCentre(long id, PollingCentre pollingCentre)
        {
            if (id != pollingCentre.CentreId)
            {
                return BadRequest();
            }

            _context.Entry(pollingCentre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollingCentreExists(id))
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

        // POST: api/PollingCentres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PollingCentre>> PostPollingCentre(PollingCentre pollingCentre)
        {
            _context.PollingCentres.Add(pollingCentre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPollingCentre", new { id = pollingCentre.CentreId }, pollingCentre);
        }

        // DELETE: api/PollingCentres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePollingCentre(long id)
        {
            var pollingCentre = await _context.PollingCentres.FindAsync(id);
            if (pollingCentre == null)
            {
                return NotFound();
            }

            _context.PollingCentres.Remove(pollingCentre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PollingCentreExists(long id)
        {
            return _context.PollingCentres.Any(e => e.CentreId == id);
        }
    }
}
