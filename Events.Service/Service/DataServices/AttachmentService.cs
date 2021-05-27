using Events.Api.Models.General;
using Events.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Events.Service.Service.DataServices
{
    public class AttachmentService : DbServiceImpl<Attachment, Attachment>
    {

        public AttachmentService(AppDbContext ctx) : base(ctx) { }



        public override IOrderedQueryable<Attachment> GetQuery()
        => context.Attachments.OrderBy(x => x.Id);

        public override IOrderedQueryable<Attachment> GetViewQuery()
        {
            throw new NotImplementedException();
        }
    }
}
