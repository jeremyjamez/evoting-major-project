using eVotingApi.Models;
using eVotingApi.Models.DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Services
{
    public class VoteService
    {
        private readonly IMongoCollection<Vote> _votes;
        private readonly IMongoCollection<Election> _elections;
        private readonly IMongoCollection<Voter_Election> _voter_elections;

        public VoteService(IEVotingDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _votes = database.GetCollection<Vote>(settings.VoteCollectionName);
            _elections = database.GetCollection<Election>(settings.ElectionCollectionName);
            _voter_elections = database.GetCollection<Voter_Election>(settings.Voter_ElectionCollectionName);
        }

        public async Task<VoteDto> GetVote(string voteId)
        {
            var filter = Builders<Vote>.Filter.Eq("_id",  ObjectId.Parse(voteId));
            var vote = await _votes.Find(filter).FirstOrDefaultAsync();

            var electionFilter = Builders<Election>.Filter.Eq("_id", ObjectId.Parse(vote.ElectionId));
            var election = await _elections.Find(electionFilter).FirstOrDefaultAsync();

            return VoteToDto(vote, election);
        }

        public async Task<object> InsertVote(VoteDto voteDto)
        {
            try
            {
                var vote = DtoToVote(voteDto);
                await _votes.InsertOneAsync(vote);

                var voteDtoRes = await GetVote(vote.Id);

                if (voteDtoRes == null)
                    return null;

                var voter_election = DtoToVoter_Election(voteDto);
                await _voter_elections.InsertOneAsync(voter_election);

                return new { ElectionId = voter_election.ElectionId, BallotTime = voter_election.BallotTime, VoteId = vote.Id };
            }
            catch(MongoWriteException e)
            {
                return null;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<List<dynamic>> CountVotes(string electionId)
        {
            var match = new BsonDocument
            {
                {
                    "$match",
                    new BsonDocument
                    {
                        {"electionId", electionId }
                    }
                }
            };

            var group = new BsonDocument
            {
                {
                    "$group",
                    new BsonDocument
                    {
                        {"_id", 
                            new BsonDocument
                            {
                                {"candidateId", "$candidateId"},
                                {"electionId", "$electionId" }
                            }
                        },
                        {"constituencyId",
                            new BsonDocument
                            {
                                {"$first", "$constituencyId" }
                            }
                        },
                        {"count", 
                            new BsonDocument
                            {
                                {"$sum", 1 }
                            }
                        }
                    }
                }
            };

            var project = new BsonDocument
            {
                {
                    "$project",
                    new BsonDocument
                    {
                        { "_id", 0 },
                        { "candidateId", "$_id.candidateId" },
                        { "constituencyId", "$constituencyId" },
                        { "electionId", "$_id.electionId" },
                        { "totalVotes", "$count" }
                    }
                }
            };

            var pipeline = new[] { match, group, project };
            var result = await _votes.AggregateAsync<dynamic>(pipeline);
            return result.ToList();
        }

        public static VoteDto VoteToDto(Vote vote, Election election) =>
            new VoteDto
            {
                Id = vote.Id,
                ElectionId = election.Id
            };

        private static Vote DtoToVote(VoteDto voteDto) =>
            new Vote
            {
                CandidateId = voteDto.CandidateId,
                ElectionId = voteDto.ElectionId,
                ConstituencyId = voteDto.ConstituencyId
            };

        private static Voter_Election DtoToVoter_Election(VoteDto voteDto) =>
            new Voter_Election
            {
                VoterId = voteDto.VoterId,
                ElectionId = voteDto.ElectionId,
                BallotTime = voteDto.BallotTime
            };
    }
}
