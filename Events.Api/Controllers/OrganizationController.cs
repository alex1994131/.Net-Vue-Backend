
using System.Collections.Generic;
using System.Linq;
using Events.Api.Models.General;
using Events.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {

        private readonly AppDbContext _ctx;
        public OrganizationController(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        // GET: api/<OrganizationController>
        [HttpGet]
        [Authorize]
        public IList<Organization> Get()
        {

           return _ctx.Organizations.ToList();
        }

        // GET api/<OrganizationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrganizationController>
        [HttpPost]
        [Authorize]
        public Organization Post([FromBody] Organization neworg)
        {
            _ctx.Organizations.Add(neworg);
            _ctx.SaveChanges();
            return neworg;
        }

        // PUT api/<OrganizationController>/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrganizationController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
        }
    }
}
