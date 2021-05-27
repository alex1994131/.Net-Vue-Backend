using Events.Api.Models.UserManagement;
using Events.Core.Models.General;
using System;
using System.Collections.Generic;


namespace Events.Core.Models.Logging
{
    public class ChangeLog
    {

        public static ChangeLog Build(EUser user, List<ChangeLogField> fs = null) {
            ChangeLog changeLog = new ChangeLog();
            changeLog.changeDate = DateTime.Now;
            changeLog.changedBy = user;
            changeLog.fields = fs ?? new List<ChangeLogField>();

            return changeLog;
        }

        public static void AddChangeLog(EUser user, ref List<ChangeLog> list,string field, string oldValue, string newValue)
        {
            ChangeLog changeLog = Build(user);
            changeLog.addChangeField(new ChangeLogField()
            {
                FieldName = field,
                OldValue = oldValue,
                NewValue = newValue
            });

            list.Add(changeLog);
        }

        public void addChangeField(ChangeLogField cf) {
            fields.Add(cf);
        }
        public long Id { get; set; }
        public List<ChangeLogField> fields { get; set; } = new List<ChangeLogField>();
        public EUser changedBy { get; set; }
        public DateTime changeDate { get; set; }
           
    }
}
