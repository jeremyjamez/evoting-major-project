using eVotingApi.Models;
using eVotingApi.Models.DTO;
using eVotingApi.Models.DTO.Responses;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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

        /// <summary>
        /// Returns a list of all voters
        /// </summary>
        /// <returns>An enumerable of voters</returns>
        public async Task<IEnumerable<VoterDto>> Get()
        {
            //var v = await _voters.AsQueryable().Select(x => VoterToDto(x)).ToListAsync();
            return await _voters.Find(voter => true).Project(x => VoterToDto(x)).ToListAsync();
        }

        /// <summary>
        /// Returns a voter specified by the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>VoterDto</returns>
        public async Task<VoterDto> GetById(string id)
        {
            return await _voters.AsQueryable().Where(voter => voter.VoterId == id).Select(voter => VoterToDto(voter)).FirstOrDefaultAsync();
        }
        
        /// <summary>
        /// Checks if the voter which is specified by the ID and date of birth, is registered
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dob"></param>
        /// <returns>A RegisteredResponse object containing boolean values if the voter is registered and has 2FA enabled</returns>
        public async Task<RegisteredResponse> IsRegistered(VoterDto voterDto)
        {
            var builder = Builders<Voter>.Filter;
            var filter = builder.And(builder.Eq("voterId", voterDto.VoterId), builder.Eq("dateofBirth", DateTime.Parse(voterDto.DateofBirth).Date.ToString("dd/MM/yyyy")));
            var result = await _voters.Find(filter).FirstOrDefaultAsync();

            RegisteredResponse registeredResponse;

            if(result != null)
            {
                if (!result.isTwoFactorEnabled)
                {
                    registeredResponse = new RegisteredResponse
                    {
                        isRegistered = true,
                        isTwoFactorEnabled = false
                    };
                    return registeredResponse;
                }
                else
                {
                    registeredResponse = new RegisteredResponse
                    {
                        isRegistered = true,
                        isTwoFactorEnabled = true
                    };
                    return registeredResponse;
                }
            }

            return new RegisteredResponse { isRegistered = false, isTwoFactorEnabled = false };
        }

        /// <summary>
        /// Returns all voters within a constituency specified by the ID
        /// </summary>
        /// <param name="constituencyId"></param>
        /// <returns>An enumerable of voters</returns>
        public async Task<IEnumerable<Voter>> GetByConstituency(string constituency)
        {
            return await _voters.Find(voter => voter.Constituency == constituency).ToListAsync();
        }

        /// <summary>
        /// Retrieves a voter specified by the ID and returns a Data Transfer Object (DTO) containing data for security questions
        /// </summary>
        /// <param name="id"></param>
        /// <returns>SecurityQuestionsDTO</returns>
        public async Task<SecurityQuestionsDTO> GetByVoterId(string id)
        {
            return await _voters.AsQueryable().Where(voter => voter.VoterId == id).Select(voter => QuestionsToDTO(voter)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="hashedSecretCode"></param>
        /// <param name="voterId"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateHashedSecretCode(string salt, long voterId)
        {
            var filter = Builders<Voter>.Filter.Eq("voterId", voterId);
            var update = Builders<Voter>.Update.Set("salt", salt).Set("isTwoFactorEnabled", true).CurrentDate("lastModified");
            var result = await _voters.UpdateOneAsync(filter, update);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voterId"></param>
        /// <returns></returns>
        public async Task<string> GetSecretCodeSalt(string voterId)
        {
            return await _voters.AsQueryable().Where(voter => voter.VoterId == voterId).Select(voter => voter.Salt).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Data Transfer Object (DTO) which hides some properties of the Voter entity
        /// </summary>
        /// <param name="voter"></param>
        /// <returns></returns>
        private static VoterDto VoterToDto(Voter voter) =>
            new VoterDto
            {
                VoterId = voter.VoterId,
                FirstName = voter.FirstName,
                MiddleName = voter.MiddleName,
                LastName = voter.LastName,
                DateofBirth = voter.DateOfBirth
            };

        /// <summary>
        /// Data Transfer Object (DTO) which hides some properties of the Voter entity
        /// </summary>
        /// <param name="voter"></param>
        /// <returns></returns>
        private static VoterDto VoterToIDandDobDto(Voter voter) =>
            new VoterDto
            {
                VoterId = voter.VoterId,
                DateofBirth = voter.DateOfBirth
            };

        /// <summary>
        /// Data Transfer Object (DTO) which hides some properties of the Voter entity to
        /// only contain data for generating security questions
        /// </summary>
        /// <param name="voter"></param>
        /// <returns></returns>
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
