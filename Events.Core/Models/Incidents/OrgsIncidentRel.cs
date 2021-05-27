using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.Incidents
{
    public class OrgsIncidentRel
    {
        public Incident Incident { get; set; }
        public long IncidentId { get; set; }
        public Organization Organization { set; get; }
        public long OrganizationId { set; get; }

    }
}
