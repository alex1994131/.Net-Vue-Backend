using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.Incidents
{
    public class OwnerDetails : Model
    {
        public string subsId { get; set; }
        public string ownerSub { get; set; }
        public string ownerType { get; set; }
        public string cid { get; set; }
        public string phoneNum { get; set; }
    }
}
