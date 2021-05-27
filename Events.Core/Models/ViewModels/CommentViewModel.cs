using Events.Api.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.ViewModels
{
    public class CommentViewModel
    {
        public int relid { get; set; }
        public String commentString { get; set; }
        public int parentCommentId { get; set; }

        public List<Attachment> attachments { get; set; }
    }
}
