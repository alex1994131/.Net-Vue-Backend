using Events.Core.Models;

namespace Events.Api.Models.Incidents
{
    public class Port : Model
    {

        public string Source { get; set; }
        public string Dest { get; set; }
    }
}