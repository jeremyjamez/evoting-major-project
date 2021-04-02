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

namespace eVotingApi.Controllers
{
    [Authorize(Roles = "Administrator,ECJ")]
    [Route("api/PollingStations")]
    [ApiController]
    public class PollingStationsController : ControllerBase
    {
        private readonly eVotingContext _context;

        public PollingStationsController(eVotingContext context)
        {
            _context = context;
        }

        // GET: api/PollingStations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollingStation>>> GetPollingStations()
        {
            return await _context.PollingStations.ToListAsync();
        }

        // GET: api/PollingStations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PollingStation>> GetPollingStation(long id)
        {
            var pollingStation = await _context.PollingStations.FindAsync(id);

            if (pollingStation == null)
            {
                return NotFound();
            }

            return pollingStation;
        }

        // PUT: api/PollingStations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPollingStation(long id, PollingStation pollingStation)
        {
            if (id != pollingStation.StationId)
            {
                return BadRequest();
            }

            _context.Entry(pollingStation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollingStationExists(id))
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

        // POST: api/PollingStations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PollingStation>> PostPollingStation(PollingStation pollingStation)
        {
            _context.PollingStations.Add(pollingStation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPollingStation", new { id = pollingStation.StationId }, pollingStation);
        }

        // DELETE: api/PollingStations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePollingStation(long id)
        {
            var pollingStation = await _context.PollingStations.FindAsync(id);
            if (pollingStation == null)
            {
                return NotFound();
            }

            _context.PollingStations.Remove(pollingStation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PollingStationExists(long id)
        {
            return _context.PollingStations.Any(e => e.StationId == id);
        }
    }
}
