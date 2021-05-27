using System;
using System.Collections.Generic;
using System.Linq;
using Events.Api.Models.General;
using Events.Core.Models.Employees;
using Events.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {

        private readonly AppDbContext _ctx;
        public SectionsController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        // GET: api/<SectionsController>
        [HttpGet]
        [Authorize]
        public List<Department> Get()
        {
            return _ctx.Departments.Include(x => x.Sections)
                .ThenInclude(x => x.Users).ToList();
        }

        // GET api/<SectionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SectionsController>
        [HttpPost]
        [Authorize]
        public List<Section> Post([FromBody] List<Section> sections)
        {
            sections.ForEach(st =>
            {
                if (_ctx.Sections.Find(st.Id) == null)
                {
                    _ctx.Sections.Add(st);
                }
            });
            _ctx.SaveChanges();
            return _ctx.Sections.ToList();
            
        }

        // PUT api/<SectionsController>/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SectionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
