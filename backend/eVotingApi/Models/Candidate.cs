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
        public long CandidateId { get; set; }

        public long ConstituencyId { get; set; }

        public long ElectionId { get; set; }

        
        //public virtual Constituency Constituency { get; set; }
        
        //public virtual Election Election { get; set; }
        
        //public virtual Member Member { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<Vote> Votes { get; set; }
        //[JsonIgnore]
        //public virtual MemberOfParliament MemberOfParliament { get; set; }
    }
}
