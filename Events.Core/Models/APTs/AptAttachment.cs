using Events.Api.Models.APTs;
using Events.Api.Models.General;
using System.ComponentModel.DataAnnotations;

namespace Events.Core.Models.APTs
{
    public class AptAttachment
    {
        public long AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
        public long APTId { get; set; }
        public APT Apt { get; set; }
    }
}
