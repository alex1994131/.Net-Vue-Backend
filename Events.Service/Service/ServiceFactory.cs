using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Api.Models.Tasks;
using Events.Core.Models;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.Notifications;
using Events.Core.Models.Tasks;
using Events.Data;
using Events.Service.Service.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserManagment.services;

namespace Events.Service.Service
{
    public delegate List<U> ConstantEntityList<U>(AppDbContext context);
    public delegate U ConstantEntityFind<U>(AppDbContext context);
    public delegate List<U> ConstantEntityWhere<U>(AppDbContext context);

    public class ServiceFactory : IServiceFactory
    {

        private readonly Dictionary<Type, object> services;
        private readonly AppDbContext context;
        private readonly IUserService UserService;
        

        public ServiceFactory(DbServiceImpl<Incident, IncidentView> svc,AppDbContext ctx,
            DbServiceImpl<Task, Taskview> ts, DbServiceImpl<Comment, Comment> cs, DbServiceImpl<Notification, NotificationView> ns,
        DbServiceImpl<Status, Status> ss, IUserService us,DbServiceImpl<Organization, Organization> os,
            DbServiceImpl<EntityAssignment, EntityAssignment> es)
        {
            context = ctx;
            UserService = us;
            services = new Dictionary<Type, object>(){
                { typeof(Status),ss },
                { typeof(Notification),ns },
                { typeof(Incident),svc },
                { typeof(Comment),cs },
                { typeof(Task),ts },
                { typeof(Organization),os },
                { typeof(EntityAssignment),es }
            };
        }

        public U GetConstant<U>(ConstantEntityFind<U> ctx)
        => ctx(context);
        public U GetConstant<U>(ConstantEntityWhere<U> ctx)
        => ctx(context).SingleOrDefault();
        public List<U> GetConstants<U>(ConstantEntityList<U> entity)
        => entity(context);

        public async void AddConstant<U>(U entity)
        {
            context.Add(entity);
            await context.SaveChangesAsync();
        }

        public IChangeLogHelper ChangeLogHelper()
         =>new ChangeLogHelper(this);


        public INotificationHelper NotificationHelper()
        => new NotificationHelper(this,UserService);

        public DbServiceImpl<T,V> ServicOf<T,V>() where T : Model 
        =>(DbServiceImpl<T, V>) services[typeof(T)];
    }
    public interface IServiceFactory
    {
        DbServiceImpl<T,V> ServicOf<T,V>() where T : Model;
        IChangeLogHelper ChangeLogHelper();
        INotificationHelper NotificationHelper();
        List<U> GetConstants<U>(ConstantEntityList<U> entity);
        U GetConstant<U>(ConstantEntityFind<U> ctx);
        U GetConstant<U>(ConstantEntityWhere<U> ctx);
        public void AddConstant<U>(U entity);


    }
}
