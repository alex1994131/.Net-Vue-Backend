using Events.Api.Models.Tasks;
using Events.Core.Models.Tasks;
using Events.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Events.Service.Service.DataServices
{
    public class TasksService : DbServiceImpl<Task, Taskview>
    {
        public TasksService(AppDbContext ctx) : base(ctx) { }


        public override IOrderedQueryable<Task> GetQuery()
        => context.Tasks.Include(x => x.Status)
                       .Include(x => x.AssignedEmps).ThenInclude(x => x.EUser)
                       .Include(x => x.Attachments).ThenInclude(x => x.attachment)
                       .Include(x => x.ClosingReport).ThenInclude(x => x.attachments).ThenInclude(x => x.attachment)
                       .Include(x => x.TaskComments).ThenInclude(x => x.Comment).ThenInclude(x => x.CreatedBy)
                       .Include(x => x.TaskComments).ThenInclude(x => x.Comment).ThenInclude(x => x.Attachments).ThenInclude(x => x.attachment)
                       .Include(x => x.Assigned_for)
                       .Include(x => x.CreatedBy)
                       .Include(x => x.TaskType)
                       .Include(x => x.TaskComments)
            .Include(x => x.ParentTask)
                       .Include(x => x.ParentIncident)
                       .OrderByDescending(x => x.CreatedDate);
            
            
            

        public override IOrderedQueryable<Taskview> GetViewQuery()
        => context.Taskviews.OrderByDescending(x => x.createdDate);
    }
}
