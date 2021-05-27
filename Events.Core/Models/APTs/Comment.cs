using Events.Api.Models.Incidents;
using Events.Api.Models.Tasks;
using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.General;
using Events.Core.Models.Tasks;
using System;
using System.Collections.Generic;

namespace Events.Api.Models.APTs
{
    public class Comment : MainModel
    {
        public string CommentString { get; set; }
        public List<CommentAttachment> Attachments { get; set; }
        public List<Comment> Replaies { get; set; }
        public List<Tag> Tags { get; set; }
        public List<TaskComment> TaskComments { get; set; }
        public List<IncidentComment> IncidentComments { get; set; }
       
    }
}