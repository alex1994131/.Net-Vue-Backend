
using Events.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Events.Api.Authorization
{
    public class ClaimAuthHandler : AuthorizationHandler<AuthRequirements>
    {
        private readonly AppDbContext ctx;
        public ClaimAuthHandler(AppDbContext ctx)
        {
            this.ctx = ctx;
        }

             
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AuthRequirements requirement)
        {

            var userId = ctx.Users.Where(u => u.UserName == context.User.Identity.Name).SingleOrDefault().Id;

            var list = ctx.RoleClaims
            .Where(rc => rc.RoleId == ctx.UserRoles.Where(e => e.UserId == userId).SingleOrDefault().RoleId).ToList();
            var hasPermissions = list.Any(c => c.ClaimType == requirement.claim.Type && c.ClaimValue == requirement.claim.Value);

            if (hasPermissions) { context.Succeed(requirement); }

            return Task.FromResult(0);

        }
    }
}
