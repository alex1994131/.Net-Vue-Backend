using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.Incidents
{
    public class IncidentAttachment
    {
        public long AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
        public long IncidentId { get; set; }
        public Incident Incident { get; set; }
    }
}
