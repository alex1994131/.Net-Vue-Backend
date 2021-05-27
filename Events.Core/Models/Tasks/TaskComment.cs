using Events.Api.Models.APTs;
using Events.Api.Models.Tasks;
using Events.Api.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.Tasks
{
    public class TaskComment
    {
        public long TaskId { set; get; }
        public Task Task { set; get; }
        public Comment Comment { set; get; }
        public long CommentId { set; get; }
    }
}


