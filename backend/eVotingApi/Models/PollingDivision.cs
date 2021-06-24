using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eVotingApi.Models
{
    [Table("PollingDivision")]
    public class PollingDivision
    {
        [Key]
        public long DivisionId { get; set; }
        [Required]
        public long ConstituencyId { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual Constituency Constituency { get; set; }
        public virtual ICollection<PollingCentre> PollingCentres { get; set; }
    }

    [Table("PollingCentre")]
    public class PollingCentre
    {
        [Key]
        public long CentreId { get; set; }
        [Required]
        public long DivisionId { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual PollingDivision PollingDivision { get; set; }
        public virtual ICollection<PollingStation> PollingStations { get; set; }
    }

    [Table("PollingStation")]
    public class PollingStation
    {
        [Key]
        public long StationId { get; set; }
        [Required]
        public int StationNumber { get; set; }
        [Required]
        public long CentreId { get; set; }

        public virtual PollingCentre PollingCentre { get; set; }
    }
}
