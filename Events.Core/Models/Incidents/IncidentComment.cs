using Events.Api.Models.APTs;
using Events.Api.Models.UserManagement;
using System;

namespace Events.Api.Models.Incidents
{
    public class IncidentComment
    {
        public long IncidentId { set; get; }
        public Incident incident { set; get; }
        public Comment Comment { set; get; }
        public long CommentId { set; get; }
    }
}