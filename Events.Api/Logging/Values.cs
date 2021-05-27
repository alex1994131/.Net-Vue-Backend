using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.Logging
{
    internal class Values
    {

        public static readonly string DELETE_TASK = "api/Tasks/DELETE";
        public static readonly string MAKE_INCIDENT = "api/Incidents/";
        

        public static readonly Dictionary<String, KeyValuePair<String, String>>
            oldNewValuesPair = new Dictionary<string, KeyValuePair<string, string>>
    {
                {DELETE_TASK,new KeyValuePair<String,String>("task deleted","task deleted before start") },
                {MAKE_INCIDENT,new KeyValuePair<String,String>("Notification","make notification an incident") },
                {"hiash",new KeyValuePair<String,String>("Notification","Ignored Notification") },
    };
    }
}
