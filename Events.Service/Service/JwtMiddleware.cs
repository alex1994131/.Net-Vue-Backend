
using Events.Core.Models.General;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagment.services;

namespace UserManagment.Services
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            configuration = config;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
                attachUserToContext(context, token);
            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token)
        {
            
                var tokenHandler = new JwtSecurityTokenHandler();
                var secret = configuration["JWT:Secret"];
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = authSigningKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                //var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value);
                var userId =jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                var roleId = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;

            // attach user to context on successful jwt validation
            context.Items[Constants.UserId.ToString()] = userId;
            context.Items[Constants.RoleId.ToString()] = roleId;

        }
    }
}

