using Events.Api.Models.General;

namespace Events.Core.Models.General
{
    public class OrganizationContact : Model
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public long OrgId { get; set; }
        public Organization Organization { get; set; }

    }
}
