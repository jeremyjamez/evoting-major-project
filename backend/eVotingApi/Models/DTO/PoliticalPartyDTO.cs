using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models.DTO
{
    public class PoliticalPartyDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Colour { get; set; }

        public string Icon { get; set; }

        public DateTime Founded { get; set; }
    }
}
