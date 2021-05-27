using Events.Api.Models.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Events.Service.Service.DataServices
{
    public class OrgsService : DbServiceImpl<Organization, Organization>
    {
        public OrgsService(Data.AppDbContext ctx) : base(ctx) { }

        public override IOrderedQueryable<Organization> GetQuery()
        => context.Organizations.OrderBy(x => x.Id);

        public override IOrderedQueryable<Organization> GetViewQuery()
         => context.Organizations.OrderBy(x => x.Id);
    }
}
