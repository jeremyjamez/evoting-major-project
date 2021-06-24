using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    public class Member
    {
        [Key]
        public long MemberId { get; set; }

        [Required]
        public string Photo { get; set; }

        [Required]
        public string Prefix { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string? Suffix { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Telephone { get; set; }

        [Required]
        public DateTime DateofBirth { get; set; }

        [Required]
        public long PartyId { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public DateTime JoinDate { get; set; }

        //[JsonIgnore]
        //public virtual PoliticalParty PoliticalParty { get; set; }
       // [JsonIgnore]
        //public virtual Candidate Candidate { get; set; }
    }
}
