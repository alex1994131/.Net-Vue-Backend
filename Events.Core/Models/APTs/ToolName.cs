using Events.Api.Models.General;

namespace Events.Api.Models.APTs
{
    public class ToolName
    {
        
        public int Id { set; get; }
        public string Name { set; get; }
        public Status Status { set; get; }
        public APT apt { set; get; }
    }
}