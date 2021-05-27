using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;

using System.Collections.Generic;

namespace Events.Api.Models.General
{
    public class Organization : Model
    {
        public string Orgname { set; get; }
        public int SectorId { set; get; }
        public Sector Sector { get; set; }
        public IList<EUser> Users { get; set; }

        public IList<OrgsIncidentRel> Incidents { get; set; }
    }
}