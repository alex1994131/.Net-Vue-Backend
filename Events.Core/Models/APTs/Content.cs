using Events.Api.Models.UserManagement;
using System;

namespace Events.Api.Models.APTs
{
    public class Content
    {

        public int Id { set; get; }
        public string ContentString { set; get; }
        public EUser CreatedBy { set; get; }
        public DateTime createdDate { set; get; } = DateTime.Now;

    }
}