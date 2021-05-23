using eVotingApi.Models;
using eVotingApi.Models.DTO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Services
{
    public class CandidateService
    {
        private readonly IMongoCollection<Candidate> _candidates;
        private readonly IMongoCollection<Voter> _voters;
        private readonly IMongoCollection<Constituency> _constituencies;

        public CandidateService(IEVotingDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _candidates = database.GetCollection<Candidate>(settings.CandidateCollectionName);
            _voters = database.GetCollection<Voter>(settings.VoterCollectionName);
            _constituencies = database.GetCollection<Constituency>(settings.ConstituencyCollectionName);
        }

        public async Task<IEnumerable<CandidateDTO>> GetCandidates(string voterId)
        {
            var voterBuilder = Builders<Voter>.Filter;
            var voterFilter = voterBuilder.Eq("voterId", voterId);
            var constituencyId = await _voters.Find(voterFilter).Project(voter => voter.ConstituencyId).FirstOrDefaultAsync();

            var conBuilder = Builders<Constituency>.Filter;
            var conFilter = conBuilder.Eq("constituencyId", constituencyId);
            var constituency = await _constituencies.Find(conFilter).FirstOrDefaultAsync();

            var canBuilder = Builders<Candidate>.Filter;
            var canFilter = canBuilder.Eq("constituencyId", constituencyId);
            var result = await _candidates.Find(canFilter).Project(c => CandidateToDto(c, constituency)).ToListAsync();

            return result;
        }

        private static CandidateDTO CandidateToDto(Candidate candidate, Constituency constituency) =>
            new CandidateDTO
            {
                CandidateId = candidate.CandidateId,
                ConstituencyId = candidate.ConstituencyId,
                FullName = candidate.FullName,
                Affiliation = candidate.Affiliation,
                Parish = constituency.Parish,
                ConstituencyName = constituency.Name
            };
    }
}
