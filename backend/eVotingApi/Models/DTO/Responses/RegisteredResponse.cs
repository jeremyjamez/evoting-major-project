using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models.DTO.Responses
{
    public class RegisteredResponse
    {
        public bool IsRegistered { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
        public string PublicKey { get; set; }
        public bool HasVoted { get; set; }
    }
}
