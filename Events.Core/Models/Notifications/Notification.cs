using Events.Api.Models.General;
using Events.Api.Models.UserManagement;
using System;
using System.Collections.Generic;

namespace Events.Core.Models
{
    public class Notification : Model
    {
        public EUser CreatedBy { get; set; }
        public DateTime DateTime { get; set; }
        public long EntityId { get; set; }
        public long ParentEntityId { get; set; }
        public int EntityType { get; set; }
        public Status Status { get; set; }
        public List<NotificationOwner> NotificationOwners { get; set; }
        public int ParentEntityType { get; set; }
    }

    public class NotificationOwner : Model
    {
        public EUser employee { get; set; }
        public bool isNew { get; set; }

    }
}
