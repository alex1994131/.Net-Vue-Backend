using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.Incidents
{
    public class IncidentView
    {
        public long id { get; set; }
        public long statusId { get; set; }
        public long createdById { get; set; }
        public String subject { get; set; }
        public String statusString { get; set; }
        public String saverity { get; set; }
        public String FullName { get; set; }
        public String OrgName { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }

    }
}
