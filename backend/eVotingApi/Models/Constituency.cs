using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    public class Constituency
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("constituencyId")]
        public string ConstituencyId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("parish")]
        public string Parish { get; set; }
    }
}
