using Events.Api.Models.General;

namespace Events.Api.Models.APTs
{
    public class CommentAttachment
    {

        public long commentId { get; set; }
        public Comment comment { get; set; }
        public long attachmentId { get; set; }
        public Attachment attachment { get; set; }
    }
}