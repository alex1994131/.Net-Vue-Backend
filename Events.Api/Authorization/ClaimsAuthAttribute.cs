using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Events.Api.Authorization
{
    public class ClaimsAuthAttribute : AuthorizeAttribute
    {
        
        public Claim claim { get;  set;}
        public ClaimsAuthAttribute(string Type,string Value)
        {
            claim = new Claim(Type, Value);
            Policy = $"{"ClaimsAuth"}.{Type}.{Value}";
        }
    }
}
