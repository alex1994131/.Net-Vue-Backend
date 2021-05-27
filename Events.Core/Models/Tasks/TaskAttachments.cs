using Events.Api.Models.General;
using Events.Api.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.Tasks
{
    public class TaskAttachments
    {
        public Task task { get; set; }

        public Attachment attachment { get; set; }

        public long AttachmentId { get; set; }

        public long TaskId { get; set; }
    }
}
