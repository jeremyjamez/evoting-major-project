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
        public string CandidateId { get; set; }
        public string ConstituencyId { get; set; }
        public string ElectionId { get; set; }
        public long BallotTime { get; set; }
    }
}
