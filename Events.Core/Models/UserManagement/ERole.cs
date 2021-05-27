using Events.Api.Models.General;
using Microsoft.AspNetCore.Identity;
using System;


namespace Events.Core.Models.UserManagement
{
    public class ERole : IdentityRole<long> 
    {
        public Section section { get; set; }

    }
}
