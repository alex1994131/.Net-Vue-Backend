using Events.Api.Models.APTs;
using Events.Api.Models.Incidents;
using Events.Api.Models.Tasks;

namespace Events.Core.Models.General
{
    public class Tag
    {
        public int Id { get; set; }
        public string name { get; set; }

        public Incident incident { get; set; }

        public APT apt { get; set; }

        public Task task { get; set; }
    }
}
