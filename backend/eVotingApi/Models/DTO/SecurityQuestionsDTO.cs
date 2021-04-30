using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models.DTO
{
    public class SecurityQuestionsDTO
    {
        [Required]
        public string Address { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Required]
        public string Occupation { get; set; }
        [Required]
        public string MothersMaidenName { get; set; }
    }
}
