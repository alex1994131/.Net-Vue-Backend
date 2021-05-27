using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.Notifications;
using Events.Core.Models.Tasks;
using Events.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagment.services;

namespace Events.Service
{
    public class NotificationHelper : INotificationHelper
    {

        
        private Dictionary<int, Func<long,EUser, List<NotificationOwner>>> owners;
        private readonly IUserService UserService;
        private IServiceFactory ServiceFactory;

        public NotificationHelper(IServiceFactory sf, IUserService us)
        {
            owners = getDictionary();
            ServiceFactory = sf;
            UserService = us;
        }




        public async Task<bool> BuildNotification( EUser pUser, Status status, int EntityType, long EntityId,int ParentEntityType =0, long ParentEntityId =0)
        {
            Notification notification = new Notification();
           
            notification.DateTime = DateTime.Now;
            notification.EntityType = EntityType;
            notification.EntityId = EntityId;
            notification.CreatedBy = pUser;
            notification.ParentEntityType = ParentEntityType;
            notification.ParentEntityId = ParentEntityId;
            notification.Status = status;
            notification.NotificationOwners = owners[EntityType].Invoke(EntityId, pUser);
            await ServiceFactory.ServicOf<Notification, NotificationView>().AddItem(notification);
            return true;
        }

        public async Task<bool> BuildNotificationForUsers(EUser pUser, List<EUser> users, Status status, int EntityType, long EntityId, int ParentEntityType = 0, long ParentEntityId = 0)
        {
            Notification notification = new Notification();

            notification.DateTime = DateTime.Now;
            notification.EntityType = EntityType;
            notification.EntityId = EntityId;
            notification.CreatedBy = pUser;
            notification.ParentEntityType = ParentEntityType;
            notification.ParentEntityId = ParentEntityId;
            notification.Status = status;
            notification.NotificationOwners = users.Select(x => new NotificationOwner() { employee = x, isNew = true }).ToList();
            await ServiceFactory.ServicOf<Notification, NotificationView>().AddItem(notification);
            return true;
        }


        private List<NotificationOwner> getUsersRelatedToComment(long entityId, EUser User)
        {
            List<NotificationOwner> list = new List<NotificationOwner>();
            Comment comment = ServiceFactory.ServicOf<Comment,Comment>().Find(entityId);
            if (comment.TaskComments == null)
                list.AddRange(getUsersRelatedToTask(comment.TaskComments[0].TaskId, User));
            else
                list.AddRange(getUsersRelatedToIncident(comment.IncidentComments[0].IncidentId, User));


            return list;

        }
        private List<NotificationOwner> getUsersRelatedToTask(long entityId, EUser User)
        {
            List<NotificationOwner> list = new List<NotificationOwner>();
            list.Add(new NotificationOwner() { employee = UserService.GetHead(User.Section.Department.Id) });
            Api.Models.Tasks.Task task = ServiceFactory.ServicOf<Api.Models.Tasks.Task, TaskComment>().Find(entityId);
            list.AddRange(task.AssignedEmps.Select(x => new NotificationOwner() { employee = x.EUser, isNew = true }).ToList());
            return list;
        }
        private List<NotificationOwner> getUsersRelatedToIncident(long entityId,EUser User)
        {
            List<NotificationOwner> list = new List<NotificationOwner>();
            list.Add(new NotificationOwner() { employee = UserService.GetHead(User.Section.Department.Id) });
            list.AddRange(UserService.GetEmpsOfDepartment((long)Departments.TC)
                .Select(x => new NotificationOwner() { employee = x , isNew = true }).ToList());

            return list;

        }

        private Dictionary<int, Func<long,EUser, List<NotificationOwner>>> getDictionary()
        => new Dictionary<int, Func<long, EUser, List<NotificationOwner>>>()
        {
            {(int)EntityType.Comment, new Func<long,EUser, List<NotificationOwner>>(getUsersRelatedToComment)},
            {(int)EntityType.Task, new Func<long,EUser, List<NotificationOwner>>(getUsersRelatedToTask)},
            {(int)EntityType.Incident, new Func<long,EUser, List<NotificationOwner>>(getUsersRelatedToIncident)}
        };

    }

    public interface INotificationHelper {
        Task<bool> BuildNotification(EUser pUser, Status status, int EntityType, long EntityId, int ParentEntityType = 0, long ParentEntityId = 0);
        Task<bool> BuildNotificationForUsers(EUser pUser,List<EUser> users, Status status, int EntityType, long EntityId, int ParentEntityType = 0, long ParentEntityId = 0);
    }

}
