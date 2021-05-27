using Events.Api.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.UserManagement
{
    public class RoleClaimsViewModel : Model
    {

        public long id { get; set; }

        public string name { get; set; }

        public Section section { get; set; }

        public List<Claims> claims { get; set; }

    }
}
