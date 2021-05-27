using Events.Api.Models.UserManagement;
using Events.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Events.Service.Service
{
    public interface IChangeLogHelper
    {
        Task<bool> AddChangeLogToDb<T, V>(T model, EUser user, List<Change> changes) where T : MainModel;
        void AddChangeLogToEntity<T>(T model, EUser user, List<Change> changes) where T : MainModel;
    }
}