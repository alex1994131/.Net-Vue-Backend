using Events.Api.Models.APTs;
using Events.Core.Models.General;
using Events.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events.Service.Service.DataServices
{
    public class CommentService : DbServiceImpl<Comment, Comment>
    {
        public CommentService(AppDbContext ctx) : base(ctx) { }
        public override IOrderedQueryable<Comment> GetQuery()
        => context.Comments.Include(x => x.IncidentComments)
            .Include(x => x.TaskComments)
            .Include(x => x.Replaies)
            .OrderBy(x => x.CreatedDate);
           

        public override IOrderedQueryable<Comment> GetViewQuery()
        => context.Comments.Include(x => x.IncidentComments)
            .Include(x => x.TaskComments)
            .OrderBy(x => x.CreatedDate);

    }
}
