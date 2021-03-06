﻿using System;
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

        [Authorize(Roles = "Administrator,EOJ")]
        [HttpGet]
        public async Task<IActionResult> GetCandidates()
        {
            var candidates = await _candidateService.GetCandidates();

            if(candidates == null)
            {
                return NotFound();
            }

            return Ok(candidates);
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

        [Authorize(Roles = "Administrator,EOJ")]
        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByConstituency(string id)
        {
            var constituency = await _candidateService.GetByConstituency(id);

            if (constituency == null)
                return NotFound();

            return Ok(constituency);
        }
    }
}
