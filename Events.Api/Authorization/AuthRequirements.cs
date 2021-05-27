
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Events.Api.Authorization
{
    public class AuthRequirements : IAuthorizationRequirement
    {
        public Claim claim { get; private set; }
        public AuthRequirements(Claim c)
        {
            claim = c;
        }
    }
}
