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
    public class Voter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonId]
        public long VoterId { get; set; }
        [BsonRequired]
        public string Prefix { get; set; }
        [BsonRequired]
        public string FirstName { get; set; }
        [BsonRequired]
        public string MiddleName { get; set; }
        [BsonRequired]
        public string LastName { get; set; }
        [BsonRequired]
        public string Address { get; set; }
        [BsonRequired]
        public string Parish { get; set; }
        [BsonRequired]
        public string Gender { get; set; }
        [BsonRequired]
        public DateTime DateOfBirth { get; set; }
        [BsonRequired]
        public string Telephone { get; set; }
        [BsonRequired]
        public string Occupation { get; set; }
        [BsonRequired]
        public string MothersMaidenName { get; set; }
        [BsonRequired]
        public string PlaceOfBirth { get; set; }
        [BsonRequired]
        public string MothersPlaceOfBirth { get; set; }
        [BsonRequired]
        public string FathersPlaceOfBirth { get; set; }

        [BsonRequired]
        public long ConstituencyId { get; set; }

        //public virtual ICollection<Vote> Votes { get; set; }
        //public virtual Constituency Constituency { get; set; }
    }
}
