using Events.Api.Models.APTs;
using Events.Api.Models.Incidents;
using Events.Api.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.General
{
    public class EntityAssignment : MainModel
    {
        public EUser User { get; set; }
        public EntityStatus Status { get; set; }
        public string Request { get; set; }
        public bool IsHandeled { get; set; }
        public long IncidentId { get; set; }
        public String Response { get; set; }
         
    }
}
