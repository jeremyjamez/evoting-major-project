﻿using System;
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
        public long CandidateId { get; set; }
        public long ConstituencyId { get; set; }

        public string FullName { get; set; }

        public string Position { get; set; }

        public string Affiliation { get; set; }

        public string ConstituencyName { get; set; }

        public string Parish { get; set; }
    }
}