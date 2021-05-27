using Events.Api.Models.General;
using Events.Core.Models;
using Events.Core.Models.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Events.Api.Models.UserManagement
{
    public class EUser : IdentityUser<long>
    {
        public string FullName { get; set; }

        public Section Section { get; set; }
        public IList<TaskEmpsRel> Tasks { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsHead { get; set; }

        public bool IsSubHead { get; set; }

        public bool IsAssignedHead { get; set; }

        public bool IsAssignedSubeHead { get; set; }
    }
}
