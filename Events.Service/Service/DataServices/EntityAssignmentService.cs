using Events.Core.Models.General;
using Events.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events.Service.Service.DataServices
{
    public class EntityAssignmentService : DbServiceImpl<EntityAssignment, EntityAssignment>
    {

        public EntityAssignmentService(AppDbContext ctx) : base(ctx) { }
        public override IOrderedQueryable<EntityAssignment> GetQuery()
        => context.EntityAssignments.Include(x => x.User).OrderBy(x => x.Id);
        

        public override IOrderedQueryable<EntityAssignment> GetViewQuery()
        => context.EntityAssignments.OrderBy(x => x.Id);
    }
}
