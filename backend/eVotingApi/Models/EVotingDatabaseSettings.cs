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
    }

    public interface IEVotingDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string VoterCollectionName { get; set; }
        string ConstituencyCollectionName { get; set; }
    }
}
