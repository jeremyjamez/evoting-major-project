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

        public long ConstituencyId { get; set; }

        public string Name { get; set; }

        public string Parish { get; set; }

        /*[JsonIgnore]
        public virtual ICollection<Voter> Voters { get; set; }
        [JsonIgnore]
        public virtual ICollection<Candidate> Candidates { get; set; }
        [JsonIgnore]
        public virtual ICollection<PollingDivision> PollingDivisions { get; set; }

        
        public virtual MemberOfParliament MemberOfParliament { get; set; }*/
    }
}
