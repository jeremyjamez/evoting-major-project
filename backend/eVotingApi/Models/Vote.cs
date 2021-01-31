using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    [Table("Vote")]
    public class Vote
    {
        [Key]
        public long VoteId { get; set; }
        [Required]
        public long VoterId { get; set; }
        [Required]
        public long CandidateId { get; set; }
        [Required]
        public long ElectionId { get; set; }

        public virtual Voter Voter { get; set; }
        public virtual Election Election { get; set; }
        public virtual Candidate Candidate { get; set; }
    }
}
