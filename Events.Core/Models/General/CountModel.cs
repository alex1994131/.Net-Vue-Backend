using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.General
{
    public class CountModel : Model
    {
        //incidents
        public long incidents { get; set; }

        //ips
        public long ips { get; set; }

        //inbox
        public long inbox { get; set; }
        public long inboxOnProgress { get; set; }
        

        //notifications
        public long notifications { get; set; }

        //APT 
        public long apt { get; set; }

    }
}
