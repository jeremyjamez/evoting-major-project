using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models.DTO
{
    public class VoterDto
    {
        public long VoterId { get; set; }
        public DateTime DateofBirth { get; set; }
    }
}
