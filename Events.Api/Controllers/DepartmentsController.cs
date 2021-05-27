using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Events.Api.Models.General;
using Events.Core.Models.Employees;
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
    public class DepartmentsController : ControllerBase
    {

        private readonly AppDbContext dbContext;
        public DepartmentsController(AppDbContext contextImpl)
        {
            dbContext = contextImpl;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                return Ok(SuccessResponse<Department>.build(null, 0, dbContext.Departments.Include(x => x.Sections)
                        .ThenInclude(x => x.Users)
                        .ToList())

                    );
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    status = (int)ResponseCodes.Exception,
                    message = e.Message
                });
            }
        }

        [HttpPost("AddSections")]
        [Authorize]
        public IActionResult Post([FromBody] List<Section> sections)
        {
            try
            {
                sections.ForEach(st =>
                {
                    if (dbContext.Sections.Where(sec => sec.Name == st.Name
                       && sec.Department.Id == st.Department.Id).SingleOrDefault() == null)
                    {
                        st.Department = dbContext.Departments.Find(st.Department.Id);
                        dbContext.Sections.Add(st);
                    }
                });
                dbContext.SaveChanges();
                long id = dbContext.Sections.Max(s => s.Id);
                return Ok(SuccessResponse<Section>.build(null, id, dbContext.Sections.Include(x => x.Users).ToList()));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("AddDepartments")]
        public IActionResult AddSections([FromBody] List<Department> departments)
        {
            try
            {
                departments.ForEach(st =>
                {
                    if (dbContext.Departments.Find(st.Id) == null)
                    {
                        dbContext.Departments.Add(st);
                    }
                });
                dbContext.SaveChanges();
                //int id = dbContext.departments.Max(s => s.id);
                return Ok(SuccessResponse<Department>.build(null, 0, dbContext.Departments.Include(x => x.Sections).ToList()));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
    }
}
