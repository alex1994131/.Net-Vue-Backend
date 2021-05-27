using Events.Api.Models.General;

namespace Events.Api.Models.APTs
{
    public class AlternativeName
    {
        
        public int id { set; get; }
        public string Name { set; get; }
        public Status Status { set; get; }
        public string dbStatus { set; get; }
    }
}