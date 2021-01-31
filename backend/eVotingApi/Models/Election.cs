using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eVotingApi.Models
{
    [Table("Election")]
    public class Election
    {
        [Key]
        public long ElectionId { get; set; }
        [Required]
        public string ElectionType { get; set; }
        [Required]
        public DateTime ElectionDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<Vote> Votes { get; set; }
        [JsonIgnore]
        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}
