using Events.Api.Models.Tasks;
using Events.Api.Models.UserManagement;

namespace Events.Core.Models.Tasks
{
    public class TaskEmpsRel 
    {
        public Task Task { get; set; }
        public EUser EUser { get; set; }
        public long EUserId { get; set; }
        public long TaskId { get; set; }
    }
}
