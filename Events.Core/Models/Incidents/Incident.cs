using Events.Api.Models.General;
using Events.Api.Models.Tasks;
using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Api.Models.Incidents
{
    public class Incident : MainModel
    {
        public Category Category { set; get; }
        public string Signature { set; get; }
        public string Description { set; get; }
        public CloseReport CloseReport { get; set; }
        public String ExtraNote1 { get; set; }
        public String ExtraNote2 { get; set; }
        public String ExtraNote3 { get; set; }
        public EntityStatus Status { set; get; }
        public string Subject { set; get; }
        public Saverity Saverity { set; get; }
        public DateTime Date { get; set; }
        public string Time { get; set; }

        public Urgancey Urgancey { set; get; }
        public List<IpAddress> IpAddresses { set; get; }
        public List<IncidentComment> Comments { set; get; }
        public List<IncidentAttachment> Attachments { set; get; }
        public List<EntityAssignment> Assignments { set; get; } = new List<EntityAssignment>();
        public List<OrgsIncidentRel> Orgs { set; get; }
        public List<Tag> Tags { set; get; }
        public bool IsIpsIdentificationRequested { get; set; }



    }
}