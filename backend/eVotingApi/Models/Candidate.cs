using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    [Table("Candidate")]
    public class Candidate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CandidateId { get; set; }
        [Required]
        public long ConstituencyId { get; set; }
        [Required]
        public long ElectionId { get; set; }

        
        public virtual Constituency Constituency { get; set; }
        
        public virtual Election Election { get; set; }
        
        public virtual Member Member { get; set; }

        [JsonIgnore]
        public virtual ICollection<Vote> Votes { get; set; }
        [JsonIgnore]
        public virtual MemberOfParliament MemberOfParliament { get; set; }
    }
}
