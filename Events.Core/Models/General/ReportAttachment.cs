using Events.Api.Models.Tasks;

namespace Events.Api.Models.General
{
    public class ReportAttachment
    {
        public CloseReport closeReport { get; set; }

        public Attachment attachment { get; set; }

        public long attachmentId { get; set; }

        public long closeReportId { get; set; }
    }
}