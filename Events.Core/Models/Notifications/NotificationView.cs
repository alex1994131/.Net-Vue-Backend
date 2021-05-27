using System;

namespace Events.Core.Models.Notifications
{
    public class NotificationView
    {
        public long NotificationId { get; set; }
        public long NotificationOwnerId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public DateTime DateTime { get; set; }
        public long StatusId { get; set; }
        public bool NotificationStatus { get; set; }
        public String FullName { get; set; }
        public int EntityType { get; set; }
        public long EntityId { get; set; }
        public String EntityTitle { get; set; }
        public long OwnerId { get; set; }

    }
}
