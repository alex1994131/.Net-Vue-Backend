using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.General
{
    public class SearchModel
    {
        public string key { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string status { get; set; }
        public string employees { get; set; }
        public string departments { get; set; }
    }
}
