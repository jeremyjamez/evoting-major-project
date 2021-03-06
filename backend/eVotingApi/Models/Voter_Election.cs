﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    public class Voter_Election
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("voterId")]
        public string VoterId { get; set; }

        [BsonElement("electionId")]
        public string ElectionId { get; set; }

        [BsonElement("ballotTime")]
        public long BallotTime { get; set; }
    }
}
