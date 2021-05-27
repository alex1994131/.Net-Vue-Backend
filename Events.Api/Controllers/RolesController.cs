using System;
using System.Collections.Generic;
using System.Linq;
using Events.Core.Models.General;
using Events.Core.Models.UserManagement;
using Events.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly RoleManager<ERole> roleManager;
        private readonly AppDbContext appContext;
        public RolesController(RoleManager<ERole> dbContext,AppDbContext context)
        {
            roleManager = dbContext;
            appContext = context;
        }
        // GET: api/Roles
        [HttpGet]
        public ActionResult Get()
        {
            
            try {
                var roles = roleManager.Roles.Include(x => x.section).ToList();

                List<RoleClaimsViewModel> list = getRolesWithClaims(roles);
                roles.ForEach(role =>
                {
                    if (list.Where(r => r.id == role.Id).ToList().Count == 0)
                        list.Add(new RoleClaimsViewModel()
                        {claims = new List<Claims>(), id = role.Id, name=role.Name, section = role.section});
                });
                     

                return Ok(SuccessResponse<RoleClaimsViewModel>.build(null, 0, list));
            } catch (Exception e)
            { return Ok(FailedResponse.Build(e.Message)); }
        }

        private List<RoleClaimsViewModel> getRolesWithClaims(List<ERole> roles)
        {
            // it will return a role with all the claims

              return appContext.RoleClaims.ToList()
                    .GroupBy(x => x.RoleId)
                    .ToDictionary(g => g.Key, g => getClaimTypeDictionary(g.ToList()))
                    .Select(claim =>
                    {
                        ERole role = roles.Where(r => r.Id == claim.Key).FirstOrDefault();
                        RoleClaimsViewModel rc = new RoleClaimsViewModel();
                        rc.claims = getClaimsFromDictionary(claim);
                        rc.id = role.Id;
                        rc.name = role.Name;
                        rc.section = role.section;
                        return rc;
                    }).ToList();
        }

        private List<Claims> getClaimsFromDictionary(KeyValuePair<long, Dictionary<string, List<string>>> claim)
        {
            return claim.Value
                 .Select(value => new Claims() { type = value.Key, values = value.Value })
                 .ToList();
        }




        private Dictionary<string, List<string>> getClaimTypeDictionary( List<IdentityRoleClaim<long>> list)
        {
            // to make it type (like incidents) with all operations (add,notift, etc ...)
            // it will return 
            return list.GroupBy(x => x.ClaimType)
                .ToDictionary(g => g.Key , y => y.Select(c => c.ClaimValue).ToList());
                
        }



        // POST: api/Roles
        [HttpPost("Claims")]
        public object Claims([FromBody] List<RoleClaimsViewModel> roles)
        {
            try
            {

                roles.ForEach(role =>
                {

                    List<IdentityRoleClaim<long>> existClaims = appContext.RoleClaims.Where(x => x.RoleId == role.id).ToList();
                    ERole eRole = appContext.Roles.Find(role.id);
                    role.claims.ForEach(claim =>
                    {
                        claim.values.ForEach(value =>
                        {
                            if (!isExist(role.id, claim.type, value, ref existClaims))
                                appContext.RoleClaims.Add(new IdentityRoleClaim<long> { RoleId = role.id, ClaimType = claim.type, ClaimValue = value });
                        });
                    });
                    appContext.RoleClaims.RemoveRange(existClaims);
                });
                appContext.SaveChanges();
                var rolesList = appContext.Roles.Include(x => x.section).ToList();
                return Ok(SuccessResponse<ERole>.build(null, 0, null));

            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        private bool isExist(long id, string type, string value,ref List<IdentityRoleClaim<long>> list)
        {
            IdentityRoleClaim<long> identityRoleClaim = list.Where(x => x.RoleId == id && x.ClaimType == type && x.ClaimValue == value).SingleOrDefault();
            bool isExist = identityRoleClaim != null;
            
            if (isExist)
                list.Remove(identityRoleClaim);

            return isExist;
        }

        public IActionResult Post([FromBody] List<ERole> roles)
        {
            try
            {

                roles.ForEach(role =>
                {
                    bool v = false;
                    if (!v)
                    {
                        role.section = appContext.Sections.Find(role.section.Id);
                        appContext.Roles.Add(role);
                        //await roleManager.CreateAsync(role);
                    }
                });
                appContext.SaveChanges();
                var rolesList = appContext.Roles.Include(x => x.section).ToList();
                return Ok(SuccessResponse<ERole>.build(null, 0, roles));

            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
