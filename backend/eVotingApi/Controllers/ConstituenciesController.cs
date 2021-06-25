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
using eVotingApi.Services;

namespace eVotingApi.Controllers
{
    [Authorize(Roles = "Administrator,EOJ")]
    [Route("api/Constituencies")]
    [ApiController]
    public class ConstituenciesController : ControllerBase
    {
        //private readonly eVotingContext _context;
        private readonly ConstituencyService _constituencyService;

        public ConstituenciesController(ConstituencyService constituencyService)
        {
            _constituencyService = constituencyService;
        }

        // GET: api/Constituencies
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Constituency>>> GetConstituencies()
        {
            return Ok(await _constituencyService.Get());
        }

        // GET: api/Constituencies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Constituency>> GetConstituency(string id)
        {
            var constituency = await _constituencyService.GetById(id);

            if (constituency == null)
            {
                return NotFound();
            }

            return constituency;
        }

        // GET: api/Constituencies/Hanover Western
        [Route("[action]/{name}")]
        [HttpGet]
        public async Task<ActionResult<Constituency>> GetConstituencyByName(string name)
        {
            var constituency = await _constituencyService.GetByName(name);

            if (constituency == null)
            {
                return NotFound();
            }

            return constituency;
        }
    }
}
