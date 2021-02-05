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

namespace eVotingApi.Controllers
{
    [Route("api/Members")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly eVotingContext _context;

        public MembersController(eVotingContext context)
        {
            _context = context;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembers()
        {
            return await _context.Members
                .Include(m => m.PoliticalParty)
                .Select(x => MemberToDTO(x))
                .ToListAsync();
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDTO>> GetMember(long id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return MemberToDTO(member);
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(long id, MemberDTO memberDTO)
        {
            if (id != memberDTO.MemberId)
            {
                return BadRequest();
            }

            var member = await _context.Members.FindAsync(id);

            if (member == null)
                return NotFound();

            member.Photo = memberDTO.Photo;
            member.Prefix = memberDTO.Prefix;
            member.FirstName = memberDTO.FirstName;
            member.MiddleName = memberDTO.MiddleName;
            member.LastName = memberDTO.LastName;
            member.Position = memberDTO.Position;
            member.PartyId = memberDTO.PartyId;
            member.Telephone = memberDTO.Telephone;
            member.Address = memberDTO.Address;

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MemberExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(MemberDTO memberDTO)
        {
            var member = new Member
            {
                Photo = memberDTO.Photo,
                Prefix = memberDTO.Prefix,
                FirstName = memberDTO.FirstName,
                MiddleName = memberDTO.MiddleName,
                LastName = memberDTO.LastName,
                Suffix = memberDTO.Suffix,
                Gender = memberDTO.Gender,
                Address = memberDTO.Address,
                Telephone = memberDTO.Telephone,
                DateofBirth = memberDTO.DateofBirth,
                PartyId = memberDTO.PartyId,
                Position = memberDTO.Position,
                JoinDate = memberDTO.JoinDate
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.MemberId }, MemberToDTO(member));
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(long id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(long id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }

        private static MemberDTO MemberToDTO(Member member) =>
            new MemberDTO
            {
                MemberId = member.MemberId,
                Photo = member.Photo,
                Prefix = member.Prefix,
                FirstName = member.FirstName,
                MiddleName = member.MiddleName,
                LastName = member.LastName,
                Suffix = member.Suffix,
                Gender = member.Gender,
                Address = member.Address,
                Telephone = member.Telephone,
                DateofBirth = member.DateofBirth,
                PartyId = member.PartyId,
                Position = member.Position,
                JoinDate = member.JoinDate,
                PoliticalParty = member.PoliticalParty
            };
    }
}
