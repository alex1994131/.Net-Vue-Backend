using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.General;
using Events.Core.Models.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Events.Service.Service
{
    public class ChangeLogHelper : IChangeLogHelper
    {
        private IServiceFactory serviceFactory;

        public ChangeLogHelper(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public async Task<bool> AddChangeLogToDb<T,V>(T model, EUser user, List<Change> changes) where T : MainModel
        {

            ChangeLog changeLog = ChangeLog.Build(user);
            foreach(Change change in changes)
                changeLog.addChangeField(new ChangeLogField()
                {
                    FieldName = change.Field,
                    OldValue = change.OldValue,
                    NewValue = change.newValue
                });
            model.Changes.Add(changeLog);
            await serviceFactory.ServicOf<T, V>().UpdateEntity(model);
            return true;
        }

        public void AddChangeLogToEntity<T>(T model, EUser user, List<Change> changes) where T : MainModel
        {
            ChangeLog changeLog2 = ChangeLog.Build(user);
            foreach(Change change in changes)
                changeLog2.addChangeField(new ChangeLogField()
                {
                    FieldName = change.Field,
                    OldValue = change.OldValue,
                    NewValue = change.newValue
                });
            model.Changes.Add(changeLog2);
        }
    }
}
