using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models.DTO.Responses
{
    public class RegisteredResponse
    {
        public bool isRegistered { get; set; }
        public bool isTwoFactorEnabled { get; set; }
        public string PublicKey { get; set; }
    }
}
