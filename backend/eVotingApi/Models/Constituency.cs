using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    [Table("Constituency")]
    public class Constituency
    {
        [Key]
        public long ConstituencyId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Parish { get; set; }

        [JsonIgnore]
        public virtual ICollection<Voter> Voters { get; set; }
        [JsonIgnore]
        public virtual ICollection<Candidate> Candidates { get; set; }
        [JsonIgnore]
        public virtual ICollection<PollingDivision> PollingDivisions { get; set; }

        
        public virtual MemberOfParliament MemberOfParliament { get; set; }
    }
}
