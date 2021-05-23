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
    public class Party
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("partyId")]
        public string PartyId { get; set; }

        [BsonElement("longName")]
        public string LongName { get; set; }

        [BsonElement("shortName")]
        public string ShortName { get; set; }

        [BsonElement("colour")]
        public string Colour { get; set; }

        [BsonElement("icon")]
        public string Icon { get; set; }
    }
}
