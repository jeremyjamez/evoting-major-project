using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    public class EVotingDatabaseSettings : IEVotingDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string VoterCollectionName { get; set; }
        public string ConstituencyCollectionName { get; set; }
        public string CandidateCollectionName { get; set; }
        public string PartyCollectionName { get; set; }
        public string ElectionCollectionName { get; set; }
        public string VoteCollectionName { get; set; }
        public string Voter_ElectionCollectionName { get; set; }
    }

    public interface IEVotingDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string VoterCollectionName { get; set; }
        string ConstituencyCollectionName { get; set; }
        string CandidateCollectionName { get; set; }
        string PartyCollectionName { get; set; }
        string ElectionCollectionName { get; set; }
        string VoteCollectionName { get; set; }
        string Voter_ElectionCollectionName { get; set; }
    }
}
