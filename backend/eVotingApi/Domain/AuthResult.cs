using eVotingApi.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Domain
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public ICollection<string> Errors { get; set; }
    }
}
