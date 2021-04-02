using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models.Auth
{
    public class Role : IdentityRole<Guid>
    {
        [Required]
        public string Description { get; set; }
    }
}
