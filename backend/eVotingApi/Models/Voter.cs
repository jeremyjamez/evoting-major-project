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

        [BsonRequired]
        [BsonElement("voterId")]
        public string VoterId { get; set; }

        [BsonRequired]
        [BsonElement("prefix")]
        public string Prefix { get; set; }

        [BsonRequired]
        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonRequired]
        [BsonElement("middleName")]
        public string MiddleName { get; set; }

        [BsonRequired]
        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonRequired]
        [BsonElement("address")]
        public string Address { get; set; }

        [BsonRequired]
        [BsonElement("parish")]
        public string Parish { get; set; }

        [BsonRequired]
        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonRequired]
        [BsonElement("dateofBirth")]
        public string DateOfBirth { get; set; }

        [BsonRequired]
        [BsonElement("telephone")]
        public string Telephone { get; set; }

        [BsonRequired]
        [BsonElement("occupation")]
        public string Occupation { get; set; }

        [BsonRequired]
        [BsonElement("mothersMaidenName")]
        public string MothersMaidenName { get; set; }

        [BsonRequired]
        [BsonElement("placeofBirth")]
        public string PlaceOfBirth { get; set; }

        [BsonRequired]
        [BsonElement("mothersPlaceofBirth")]
        public string MothersPlaceOfBirth { get; set; }

        [BsonRequired]
        [BsonElement("maritalStatus")]
        public string MaritalStatus { get; set; }

        [BsonRequired]
        [BsonElement("constituencyId")]
        public string ConstituencyId { get; set; }

        [BsonElement("salt")]
        public string Salt { get; set; }

        [BsonElement("isTwoFactorEnabled")]
        public bool isTwoFactorEnabled { get; set; }
    }
}
