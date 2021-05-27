
using Events.Api.Models.APTs;
using Events.Api.Models.Tasks;
using Events.Core.Models;
using Events.Core.Models.APTs;
using Events.Core.Models.Incidents;
using System.Collections.Generic;

namespace Events.Api.Models.General
{
    public class Attachment : Model
    {

        public string Title { get; set; }
        public string Extension { get; set; }
        public string Filename { get; set; }
        public string Type { get; set; }
        public byte[] Content { get; set; }
        public string Url { get; set; }
        public bool Is64base { get; set; }
        public IList<AptAttachment> aptAttachment { get; set; }
        public IList<IncidentAttachment> incidentAttachments { get; set; }
        public IList<ReportAttachment> reportAttachmanet { get; set; }
        public IList<CommentAttachment> commentAttachments { get; set; }
    }
}
