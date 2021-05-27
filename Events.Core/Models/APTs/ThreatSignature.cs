using System.ComponentModel.DataAnnotations;

namespace Events.Api.Models.APTs
{
    public class ThreatSignature
    {
        [Key]
        public int Id { set; get; }
        public int Serial { set; get; }
        public string Name { set; get; }
        public string DomainName { set; get; }
    }

}
