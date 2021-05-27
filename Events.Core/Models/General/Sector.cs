using Events.Core.Models;
using System.Collections.Generic;

namespace Events.Api.Models.General
{
    public class Sector: Model
    {
        public string Name { get; set; }
        public IList<Organization> Organizations { get; set; }
    }
}
