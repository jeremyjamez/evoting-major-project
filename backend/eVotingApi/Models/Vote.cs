using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    public class Vote
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("voterId")]
        public string VoterId { get; set; }
        [BsonElement("candidateId")]
        public string CandidateId { get; set; }
        [BsonElement("electionId")]
        public string ElectionId { get; set; }
        [BsonElement("constituencyId")]
        public string ConstituencyId { get; set; }
        [BsonElement("ballotTime")]
        public string BallotTime { get; set; }
    }
}
