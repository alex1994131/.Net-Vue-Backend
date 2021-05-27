using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.Incidents
{
    public class ChangesModel
    {
        public int id { get; set; }
        public int changeType { get; set; }

        public string reasonString { get; set; }
      

    }
}
