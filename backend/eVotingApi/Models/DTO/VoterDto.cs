using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eVotingApi.Models.DTO
{
    public class VoterDto
    {
        public string VoterId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DateofBirth { get; set; }
        public string PublicKey { get; set; }
        public long CurrentTime { get; set; }

        [JsonIgnore]
        public string FullName { get => string.Format("{0} {1} {2}", FirstName, MiddleName, LastName); }
    }
}
