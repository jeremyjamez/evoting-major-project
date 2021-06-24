using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models.DTO
{
    public class VoteDto
    {
        public string Id { get; set; }
        public string VoterId { get; set; }
        public string VoterName { get; set; }
        public string Election { get; set; }
        public string ConstituencyName { get; set; }
        public string BallotTime { get; set; }
    }
}
