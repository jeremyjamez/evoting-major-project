using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models.DTO
{
    public class ElectionDTO
    {
        public long ElectionId { get; set; }
        [Required]
        public string ElectionType { get; set; }
        [Required]
        public DateTime ElectionDate { get; set; }
    }
}
