﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eVotingApi.Models;

namespace eVotingApi.Controllers
{
    [Route("api/Constituencies")]
    [ApiController]
    public class ConstituenciesController : ControllerBase
    {
        private readonly eVotingContext _context;

        public ConstituenciesController(eVotingContext context)
        {
            _context = context;
        }

        // GET: api/Constituencies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Constituency>>> GetConstituencies()
        {
            return await _context.Constituencies
                .Include(x => x.MemberOfParliament)
                .ToListAsync();
        }

        // GET: api/Constituencies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Constituency>> GetConstituency(long id)
        {
            var constituency = await _context.Constituencies.FindAsync(id);

            if (constituency == null)
            {
                return NotFound();
            }

            return constituency;
        }

        // GET: api/Constituencies/GetByName/Hanover Western
        [Route("[action]/{name}")]
        [HttpGet]
        public async Task<ActionResult<Constituency>> GetByName(string name)
        {
            var constituency = await _context.Constituencies.Where(c => c.Name.Equals(name)).FirstAsync();

            if (constituency == null)
            {
                return NotFound();
            }

            return constituency;
        }

        // PUT: api/Constituencies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConstituency(long id, Constituency constituency)
        {
            if (id != constituency.ConstituencyId)
            {
                return BadRequest();
            }

            _context.Entry(constituency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConstituencyExists(id))
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

        // POST: api/Constituencies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Constituency>> PostConstituency(Constituency constituency)
        {
            _context.Constituencies.Add(constituency);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConstituency", new { id = constituency.ConstituencyId }, constituency);
        }

        // DELETE: api/Constituencies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConstituency(long id)
        {
            var constituency = await _context.Constituencies.FindAsync(id);
            if (constituency == null)
            {
                return NotFound();
            }

            _context.Constituencies.Remove(constituency);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConstituencyExists(long id)
        {
            return _context.Constituencies.Any(e => e.ConstituencyId == id);
        }
    }
}