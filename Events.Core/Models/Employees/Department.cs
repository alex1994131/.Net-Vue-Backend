using Events.Api.Models.General;
using System.Collections.Generic;

namespace Events.Core.Models.Employees
{
    public class Department : Model
    {
        public string Name { get; set; }
        public IList<Section> Sections { get; set; }
    }
}
