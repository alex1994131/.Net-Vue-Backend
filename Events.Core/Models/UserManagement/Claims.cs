using System.Collections.Generic;

namespace Events.Core.Models.UserManagement
{
    public class Claims
    {
        public string type { get; set; }

        public List<string> values { get; set; }
    }
}