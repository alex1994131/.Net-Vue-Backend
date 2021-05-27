using System;

namespace Events.Core.Models.Tasks
{
    public class Taskview 
    {
        public long TaskId { get; set; }
        public string sectionname { get; set;}
        public string createdByDepartment { get; set; }
        public long createdByDepartmentId { get; set; }
        public string assignedForDepartment { get; set;}
        public long assignedForDepartmentId { get; set; }
        public string description { get; set;}
        public string title { get; set;}
        public string dueDate { get; set;}
        public string tasktype { get; set;}
        public DateTime createdDate { get; set;}
        public int importance { get; set;} 
        public long statusId { get; set;}
        public long createdById { get; set;}
        public long asignedforid { get; set;}
        public int urgent { get; set;}
        public string StatusString { get; set;}
        public string assignedForName { get; set;}
        public string createdBy { get; set;}
        public string date { get; set;}
        public long parentIncidentid { get; set;}
        public long parentTaskid { get; set;} 
        public long closingReportid { get; set;}
        public int progress { get; set;}
    }
}