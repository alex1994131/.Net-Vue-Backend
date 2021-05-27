using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.Incidents
{
    public class AssignModelIncident
    {
        public List<Int64> Users { get; set; }

        public long Incident { get; set; }

        public long AssignmentId { get; set; }

        public String Request { get; set; }

        public String Response { get; set; }

    }
}
