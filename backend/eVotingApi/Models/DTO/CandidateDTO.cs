using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eVotingApi.Models.DTO
{
    public class CandidateDTO
    {
        public string CandidateId { get; set; }

        public string ConstituencyId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Affiliation { get; set; }

        public string Photo { get; set; }

        public string ConstituencyName { get; set; }

        public string Parish { get; set; }
    }
}
