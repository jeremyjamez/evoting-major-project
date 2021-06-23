using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models.DTO
{
    public class ElectionDTO
    {
        public string ElectionId { get; set; }

        public string ElectionType { get; set; }

        public DateTime ElectionDate { get; set; }

        public string ElectionTitle { get => string.Format("{0} {1}", ElectionDate.Year, ElectionType); }
    }
}
