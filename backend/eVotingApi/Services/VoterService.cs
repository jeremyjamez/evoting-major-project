using eVotingApi.Models;
using eVotingApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Services
{
    public class VoterService
    {
        private readonly IMongoCollection<Voter> _voters;

        public VoterService(IEVotingDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _voters = database.GetCollection<Voter>(settings.VoterCollectionName);
        }

        public async Task<List<VoterDto>> Get()
        {
            return await _voters.AsQueryable().Select(x => VoterToDto(x)).ToListAsync();
        }

        public async Task<VoterDto> GetById(long id)
        {
            return await _voters.AsQueryable().Where(voter => voter.VoterId == id).Select(voter => VoterToDto(voter)).FirstOrDefaultAsync();
        }
        
        public async Task<Voter> GetByIdandDob(long id, string dob)
        {
            return await _voters.Find(voter => voter.VoterId == id && voter.DateOfBirth.Equals(DateTime.Parse(dob))).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Voter>> GetByConstituencyId(long id)
        {
            return await _voters.Find(voter => voter.ConstituencyId == id).ToListAsync();
        }

        public async Task<SecurityQuestionsDTO> GetByVoterId(long id)
        {
            var questions = await _voters.AsQueryable().Where(voter => voter.VoterId == id).Select(voter => QuestionsToDTO(voter)).FirstOrDefaultAsync();

            return questions;
        }

        private static VoterDto VoterToDto(Voter voter) =>
            new VoterDto
            {
                VoterId = voter.VoterId,
                DateofBirth = voter.DateOfBirth
            };

        private static SecurityQuestionsDTO QuestionsToDTO(Voter voter) =>
            new SecurityQuestionsDTO
            {
                Address = voter.Address,
                DateOfBirth = voter.DateOfBirth,
                Telephone = voter.Telephone,
                Occupation = voter.Occupation,
                MothersMaidenName = voter.MothersMaidenName
            };
    }
}
