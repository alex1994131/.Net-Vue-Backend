using Events.Api.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.Logging
{
    public class UserActivity
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public EUser UserName { get; set; }
        public string IPAddress { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
