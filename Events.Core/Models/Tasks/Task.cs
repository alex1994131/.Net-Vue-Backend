using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.Logging;
using Events.Core.Models.Tasks;
using System;
using System.Collections.Generic;

namespace Events.Api.Models.Tasks
{
    public class Task : MainModel
    {

        public string Description { get; set; }
        public TaskType TaskType { get; set; }
        public string Title { get; set; }
        public int Importance { get; set; }
        public int Urgent { get; set; }
        public Status Status { get; set; }
        public Section Assigned_for { get; set; }
        public List<TaskEmpsRel> AssignedEmps { get; set; }
        public List<TaskAttachments> Attachments { get; set; }
        public int Weight { get; set; }
        public string Date { get; set; }
        public string DueDate { get; set; }
        public List<TaskComment> TaskComments { get; set; }
        public Incident ParentIncident { get; set; }
        public CloseReport ClosingReport { get; set; }
        public Task ParentTask { get; set; }
        public bool IsIncident { get; set; }
        public int Progress { get; set; }
        public int Rate { get; set; }
    }
}