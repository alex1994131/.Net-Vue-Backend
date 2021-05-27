using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.IpIdentification

{
    public class IdentificationRequest
    {
        public long id { set; get; }
        public string docmentId { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string source { get; set; }
        public string dist { get; set; }
        public string fromPort { get; set; }
        public string ownerType { get; set; }
        public string toPort { get; set; }
        public string aptGroup { get; set; }
        public string subsId { get; set; }
        public string ownerSub { get; set; }
        public string cid { get; set; }

        public bool isKnown { get; set; }
        public string phoneNum { get; set; }
    }
}
