using Events.Api.Models.APTs;
using Events.Api.Models.General;
using System.ComponentModel.DataAnnotations;

namespace Events.Core.Models.APTs
{
    public class OriginCountry
    {
        public long CountryId { get; set; }
        public Country Country { get; set; }
        public long APTId { get; set; }
        public APT Apt { get; set; }
    }
}
