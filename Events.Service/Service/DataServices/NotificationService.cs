using Events.Core.Models;
using Events.Core.Models.Notifications;
using Events.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Events.Service.Service.DataServices
{
    public class NotificationService : DbServiceImpl<Notification, NotificationView>
    {
        public NotificationService(AppDbContext ctx) : base(ctx) { }

        public override IOrderedQueryable<Notification> GetQuery()
        =>context.Notifications
                .Include(x => x.NotificationOwners)
                .ThenInclude(x=> x.employee)
                .OrderByDescending(x=> x.Id);
        

        public override IOrderedQueryable<NotificationView> GetViewQuery()
        => context.VNotifications.OrderBy(x => x.DateTime);
    }
}
