using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eVotingApi.Models;
using eVotingApi.Models.DTO;

namespace eVotingApi.Controllers
{
    [Route("api/PoliticalParties")]
    [ApiController]
    public class PoliticalPartiesController : ControllerBase
    {
        private readonly eVotingContext _context;

        public PoliticalPartiesController(eVotingContext context)
        {
            _context = context;
        }

        // GET: api/PoliticalParties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PoliticalParty>>> GetPoliticalParties()
        {
            return await _context.PoliticalParties.ToListAsync();
        }

        // GET: api/PoliticalParties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PoliticalParty>> GetPoliticalParty(long id)
        {
            var politicalParty = await _context.PoliticalParties.FindAsync(id);

            if (politicalParty == null)
            {
                return NotFound();
            }

            return politicalParty;
        }

        // PUT: api/PoliticalParties/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoliticalParty(long id, PoliticalParty politicalParty)
        {
            if (id != politicalParty.PartyId)
            {
                return BadRequest();
            }

            _context.Entry(politicalParty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoliticalPartyExists(id))
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

        // POST: api/PoliticalParties
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PoliticalPartyDTO>> PostPoliticalParty(PoliticalPartyDTO politicalPartyDTO)
        {
            var politicalParty = new PoliticalParty
            {
                Name = politicalPartyDTO.Name,
                Colour = politicalPartyDTO.Colour,
                Founded = politicalPartyDTO.Founded,
                Icon = politicalPartyDTO.Icon
            };

            _context.PoliticalParties.Add(politicalParty);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPoliticalParty", new { id = politicalParty.PartyId }, PoliticalPartyToDTO(politicalParty));
        }

        // DELETE: api/PoliticalParties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoliticalParty(long id)
        {
            var politicalParty = await _context.PoliticalParties.FindAsync(id);
            if (politicalParty == null)
            {
                return NotFound();
            }

            _context.PoliticalParties.Remove(politicalParty);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PoliticalPartyExists(long id)
        {
            return _context.PoliticalParties.Any(e => e.PartyId == id);
        }

        private static PoliticalPartyDTO PoliticalPartyToDTO(PoliticalParty politicalParty) =>
            new PoliticalPartyDTO
            {
                Name = politicalParty.Name,
                Colour = politicalParty.Colour,
                Founded = politicalParty.Founded,
                Icon = politicalParty.Icon
            };
    }
}
