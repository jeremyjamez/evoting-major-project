using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    [Table(name: "MemberOfParliament")]
    public class MemberOfParliament
    {
        public long ConstituencyId { get; set; }
        public long CandidateId { get; set; }
        public string MinisterOf { get; set; }
        [JsonIgnore]
        public virtual Constituency Constituency { get; set; }
        //[JsonIgnore]
        public virtual Candidate Candidate { get; set; }
    }
}
