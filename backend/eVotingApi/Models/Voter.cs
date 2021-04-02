using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    [Table("Voter")]
    public class Voter
    {
        [Key]
        public long VoterId { get; set; }
        [Required]
        public string Prefix { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Parish { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Required]
        public string Occupation { get; set; }
        [Required]
        public string MothersMaidenName { get; set; }
        [Required]
        public string PlaceOfBirth { get; set; }
        [Required]
        public string MothersPlaceOfBirth { get; set; }
        [Required]
        public string FathersPlaceOfBirth { get; set; }
        [Required]
        public long ConstituencyId { get; set; }

        public string Role { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
        public virtual Constituency Constituency { get; set; }
    }
}
