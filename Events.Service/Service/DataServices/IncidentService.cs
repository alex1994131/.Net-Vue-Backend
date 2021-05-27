using Events.Api.Models.Incidents;
using Events.Core.Models.Incidents;
using Events.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Events.Service.Service.DataServices
{
    public class IncidentService : DbServiceImpl<Incident,IncidentView>
    {


        public IncidentService(AppDbContext ctx) : base(ctx) { }

        public override IOrderedQueryable<Incident> GetQuery()
        {
            return context.Incidents
            .Include(x => x.Category)
            .Include(x => x.Status).ThenInclude(x => x.Status)
            .Include(x => x.Saverity)
            .Include(x => x.Urgancey)
            .Include(x => x.CloseReport)
            .Include(x => x.Assignments).ThenInclude(x => x.User)
            .Include(x => x.Orgs).ThenInclude(oi => oi.Organization)
            .Include(x => x.Changes).ThenInclude(x => x.changedBy)
            .Include(x => x.Changes).ThenInclude(x => x.fields)
            .Include(x => x.Comments).ThenInclude(oi => oi.Comment).ThenInclude(x=> x.CreatedBy)
            .Include(x => x.Comments).ThenInclude(oi => oi.Comment).ThenInclude(x => x.Replaies).ThenInclude(x => x.CreatedBy)
            .Include(x => x.IpAddresses).ThenInclude(ip => ip.Dest)
            .Include(x => x.Attachments).ThenInclude(at => at.Attachment)
            .Include(x => x.IpAddresses).ThenInclude(ip => ip.Source)
            .Include(x => x.CreatedBy)
            .OrderBy(x => x.Id);
        }


        public override IOrderedQueryable<IncidentView> GetViewQuery()
        => context.IncidentViews.OrderBy(x => x.Date);

    }
}
