using Events.Api.Models.General;
using Events.Api.Models.UserManagement;
using Events.Core.Models;
using System;
using System.Collections.Generic;

namespace Events.Api.Models.Tasks
{
    public class CloseReport : MainModel
    {
        public long reportId { get; set; }
        public string report { set; get; }
        public IList<ReportAttachment> attachments { set; get; }
        
    }
}
