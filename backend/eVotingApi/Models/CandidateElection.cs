using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    [Table("CandidateElection")]
    public class CandidateElection
    {
        public long CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }

        public long ElectionId { get; set; }
        public virtual Election Election { get; set; }
    }
}
