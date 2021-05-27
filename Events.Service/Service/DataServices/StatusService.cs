using Events.Api.Models.General;
using Events.Api.Models.UserManagement;
using Events.Core.Models.Services;
using Events.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Events.Service.Service.DataServices
{
    public class StatusService : DbServiceImpl<Status, Status>
    {

        public StatusService(AppDbContext ctx) : base(ctx) { }


        public override IOrderedQueryable<Status> GetQuery()
        => context.Statuses.OrderBy(x => x.Id);

        public override IOrderedQueryable<Status> GetViewQuery()
        => context.Statuses.OrderBy(x => x.Id);
    }
}
