using Events.Api.Models.UserManagement;
using Events.Core.Models.General;
using Events.Core.Models.UserManagement;
using Events.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UserManagment
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<EUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<ERole> roleManager;
        private readonly AppDbContext _ctx;

        public AuthenticateController(UserManager<EUser> userManager,
            IConfiguration configuration, RoleManager<ERole> rm,
            AppDbContext dbContext)
        {
            this._userManager = userManager;
            _configuration = configuration;
            _ctx = dbContext;
            roleManager = rm;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);

                if (userExists != null)
                {
                    return Ok(FailedResponse.Build("اسم المستخدم موجود مسبقا"));
                }

                var user = new EUser { Section = _ctx.Sections.Find(model.sectionId),FullName = model.Name, UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    ERole eRole = _ctx.Roles.Where(r => r.Name == model.RoleId).FirstOrDefault();
                    EUser eUser = _ctx.Users.Where(u => u.UserName == model.Username).FirstOrDefault();
                    _ctx.UserRoles.Add(new IdentityUserRole<long> { UserId = eUser.Id, RoleId = eRole.Id });
                    _ctx.SaveChanges();
                    return Ok(SuccessResponse<EUser>.build(eUser, 0, null));
                }
                else return Ok(FailedResponse.Build((result.Errors.ToList()[0].Description).ToString()));
            }
            catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            try
            {
                
                
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRole = await _userManager.GetRolesAsync(user);

                    if (userRole.Count == 0) { 
                     return Ok(FailedResponse.Build("لا يوجد صلاحيات للمستخدم"));
                    }

                    if (!user.IsEnabled)
                    {
                        return Ok(FailedResponse.Build("تم تعطيل المستخدم"));
                    }

                    string role = userRole.FirstOrDefault();
                    ERole eRole = _ctx.Roles.Where(r => r.Name == role).SingleOrDefault();

                    var menue = _ctx.Employees
                   .Where(u => u.UserName == model.Username).SingleOrDefault();
                    var claims = _ctx.RoleClaims.Where(rc => rc.RoleId == eRole.Id).ToList()
                    .GroupBy(x => x.ClaimType)
                    .ToDictionary(g => g.Key, y => y.Select(c => c.ClaimValue).ToList())
                    .Select(cg => new Claims { type = cg.Key, values = cg.Value }).ToList();



                    var secret = _configuration["JWT:Secret"];
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(10),
                        claims: new[] { new Claim(ClaimTypes.Name, model.Username) , new Claim(ClaimTypes.Role , eRole.Id.ToString()) },
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );;
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        claims,
                        data = menue,
                        status = 200
                    });
                }
                return Ok(FailedResponse.Build("كلمة المرور/اسم المستخدم خاطئة"));
            }
            catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            }
            
    
        }

        [HttpPut("updatePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordResource changePasswordResource)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = _ctx.Users.Where(x => x.UserName == username).SingleOrDefault();
                if (user == null)
                {
                    return Ok(FailedResponse.Build("اسم المستخدم غير موجود"));
                }



                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, changePasswordResource.NewPassword);
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return Ok(FailedResponse.Build(result.Errors.ToList()[0].Description));
                }
                return Ok(SuccessResponse<EUser>.build(user, 0, null));
            }
            catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPut("updatePasswordByAdmin")]
        public async Task<IActionResult> ChangePasswordByAdmin([FromBody] ChangePasswordResource changePasswordResource)
        {
            try
            {
               
                var user = _ctx.Users.Where(x => x.UserName == changePasswordResource.username).SingleOrDefault();
                if (user == null)
                {
                    return Ok(FailedResponse.Build("اسم المستخدم غير موجود"));
                }

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, changePasswordResource.NewPassword);
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return Ok(FailedResponse.Build(result.Errors.ToList()[0].Description));
                }
                return Ok(SuccessResponse<EUser>.build(user, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("disableUser")]
        public IActionResult updateEmployee([FromBody] EUser u)
        {
            try
            {

                var user = _ctx.Users.Where(x => x.Id == u.Id).SingleOrDefault();
                if (user == null)
                {
                    return Ok(FailedResponse.Build("اسم المستخدم غير موجود"));
                }
                user.IsEnabled = u.IsEnabled;
                _ctx.Entry(user).CurrentValues.SetValues(user);
                _ctx.SaveChanges();

               
                return Ok(SuccessResponse<EUser>.build(user, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("IsHeadOrNot")]
        public IActionResult isHeadOrNot ([FromBody] EUser u)
        {
            try
            {

                var user = _ctx.Users.Where(x => x.Id == u.Id).SingleOrDefault();
                if (user == null)
                {
                    return Ok(FailedResponse.Build("اسم المستخدم غير موجود"));
                }
                user.IsHead = u.IsHead;
                _ctx.Entry(user).CurrentValues.SetValues(user);
                _ctx.SaveChanges();


                return Ok(SuccessResponse<EUser>.build(user, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPut("updateEmployee")]
        public async Task<IActionResult> updateEmployeeDetails([FromBody] UserUpdateModel userUpdate)
        {
            try
            {

                var user = _ctx.Users.Find(userUpdate.id);
                if (user == null)
                {
                    return Ok(FailedResponse.Build("اسم المستخدم غير موجود"));
                }
                user.UserName = userUpdate.Username;
                user.Section = _ctx.Sections.Find(userUpdate.sectionId);
                user.FullName = userUpdate.Fullname;
                user.IsEnabled = userUpdate.isEnabled;
                user.IsHead = userUpdate.isHead;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded && userUpdate.RoleId != "")
                {
                    ERole eRole = _ctx.Roles.Where(r => r.Name == userUpdate.RoleId).FirstOrDefault();
                    
                    IdentityUserRole<long> identityUserRole = _ctx.UserRoles.Where(x => x.UserId == user.Id).SingleOrDefault();
                    if (identityUserRole != null && eRole.Id == identityUserRole.RoleId)
                        return Ok(SuccessResponse<EUser>.build(user, 0, null));
                    if (identityUserRole != null)
                        _ctx.UserRoles.Remove(identityUserRole);
                    _ctx.UserRoles.Add(new IdentityUserRole<long> { UserId = user.Id, RoleId = eRole.Id });
                    _ctx.SaveChanges();
                    return Ok(FailedResponse.Build(result.Errors.ToList()[0].Description));
                }
                return Ok(SuccessResponse<EUser>.build(user, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

    }
}
