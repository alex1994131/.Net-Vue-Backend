
using System;
using System.Collections.Generic;
using System.Linq;
using Events.Api.Models.UserManagement;
using Events.Core.Models.General;
using Events.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly AppDbContext context;
        public EmployeeController(AppDbContext impl)
        {
            context = impl;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        [Authorize ]
        public IActionResult Get()
        {
            try { 
            
            var list = context.Users
                    .Include(x => x.Section)
                    .ThenInclude(s => s.Department)
                    .Select(user => 

                    new RegisterModel() { 
                    Username = user.UserName,
                    Name = user.FullName,
                    sectionId = user.Section.Id,
                    isEnabled = user.IsEnabled,
                    isHead = user.IsHead,
                    DepartmentId = user.Section.Department.Id,
                    RoleId = context.UserRoles.Where(c => c.UserId == user.Id).SingleOrDefault().RoleId.ToString(),
                    Email  = user.Email,
                    Id = user.Id,
                    Section = user.Section.Name,
                    Department = user.Section.Department.Name
                    }
                    )
                    .ToList();

                return Ok(SuccessResponse<RegisterModel>.build(null, 0, list));
            } catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("GiveSubstitute")]
        public string GiveSubstitute(int id)
        {
            return "value";
        }


        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
