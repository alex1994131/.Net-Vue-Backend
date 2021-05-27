using Events.Core.Models;

namespace Events.Api.Models.Incidents
{
    public class Ip : Model
    {
        public string ip { get; set; }
        public string port { get; set; }
    }
}