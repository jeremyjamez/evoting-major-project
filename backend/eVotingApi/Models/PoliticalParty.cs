using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    [Table(name: "PoliticalParty")]
    public class PoliticalParty
    {
        [Key]
        public long PartyId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Colour { get; set; }

        [Required]
        public string Icon { get; set; }

        [Required]
        public DateTime Founded { get; set; }

        [JsonIgnore]
        public virtual ICollection<Member> Members { get; set; }
    }
}
