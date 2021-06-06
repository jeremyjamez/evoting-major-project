using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eVotingApi.Models
{
    public class Election
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("startTime")]
        public string StartTime { get; set; }

        [BsonElement("endTime")]
        public string EndTime { get; set; }
    }
}
