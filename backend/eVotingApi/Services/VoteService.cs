using eVotingApi.Models;
using eVotingApi.Models.DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Services
{
    public class VoteService
    {
        private readonly IMongoCollection<Vote> _votes;
        private readonly IMongoCollection<Voter> _voters;
        private readonly IMongoCollection<Election> _elections;
        private readonly IMongoCollection<Constituency> _constituencies;

        public VoteService(IEVotingDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _votes = database.GetCollection<Vote>(settings.VoteCollectionName);
            _voters = database.GetCollection<Voter>(settings.VoterCollectionName);
            _elections = database.GetCollection<Election>(settings.ElectionCollectionName);
            _constituencies = database.GetCollection<Constituency>(settings.ConstituencyCollectionName);
        }

        public async Task<VoteDto> GetVote(string voteId)
        {
            var filter = Builders<Vote>.Filter.Eq("_id",  ObjectId.Parse(voteId));
            var vote = await _votes.Find(filter).FirstOrDefaultAsync();

            var voterFilter = Builders<Voter>.Filter.Eq("voterId", vote.VoterId);
            var voter = await _voters.Find(voterFilter).FirstOrDefaultAsync();

            var electionFilter = Builders<Election>.Filter.Eq("_id", ObjectId.Parse(vote.ElectionId));
            var election = await _elections.Find(electionFilter).FirstOrDefaultAsync();

            var conFilter = Builders<Constituency>.Filter.Eq("constituencyId", vote.ConstituencyId);
            var constituency = await _constituencies.Find(conFilter).FirstOrDefaultAsync();

            return VoteToDto(vote, voter, election, constituency);
        }

        public async Task<string> InsertVote(Vote vote)
        {
            try
            {
                await _votes.InsertOneAsync(vote);
                return vote.Id;
            }
            catch(MongoWriteException e)
            {
                return "";
            }
            catch(Exception e)
            {
                return "";
            }
        }

        public static VoteDto VoteToDto(Vote vote, Voter voter, Election election, Constituency constituency) =>
            new VoteDto
            {
                Id = vote.Id,
                VoterId = vote.VoterId,
                VoterName = voter.FirstName + " " + voter.MiddleName + " " + voter.LastName,
                Election = election.ElectionTitle,
                ConstituencyName = constituency.Name,
                BallotTime = vote.BallotTime
            };
    }
}
