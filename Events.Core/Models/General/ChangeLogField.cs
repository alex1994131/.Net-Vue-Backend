using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.General
{
    public class ChangeLogField : Model
    {
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Notes { get; set; }
    }
}
