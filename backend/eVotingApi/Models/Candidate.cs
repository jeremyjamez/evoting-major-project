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
    public class Candidate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("candidateId")]
        public string CandidateId { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("affiliation")]
        public string Affiliation { get; set; }

        [BsonElement("constituencyId")]
        public string ConstituencyId { get; set; }

        [BsonIgnore]
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        //public long ElectionId { get; set; }
    }
}
