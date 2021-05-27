using Events.Api.Models.APTs;
using Events.Api.Models.General;
using System.ComponentModel.DataAnnotations;

namespace Events.Core.Models.APTs
{
    public class TargetedSector
    {
        public int SectorId { get; set; }
        public Sector Sector { get; set; }
        public int AptId { get; set; }
        public APT Apt { get; set; }
    }
}
