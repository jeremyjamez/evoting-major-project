﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eVotingApi.Models.DTO.Requests
{
    public class UserLoginRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public DateTime LastLoggedIn { get; set; }
    }
}
