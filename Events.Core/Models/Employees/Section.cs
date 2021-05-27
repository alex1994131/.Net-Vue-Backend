
using Events.Api.Models.Tasks;
using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.Employees;
using System.Collections.Generic;

namespace Events.Api.Models.General
{
    public class Section : Model
    {
        public string Name { get; set; }

        public Department Department { get; set; }

        public IList<EUser> Users { get; set; }

        public IList<Task> Tasks { get; set; }

    }
}