
using System.Collections.Generic;
using System.Linq;

using Events.Api.Models.General;
using Events.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsManagemtns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {

        private readonly AppDbContext dbContext;

        public StatusController(AppDbContext impl)
        {
            dbContext = impl;
        }

        // GET: api/Status
        [HttpGet]
        [Authorize]
        public List<Status> Get()
        {
            return dbContext.Statuses.ToList();
        }

        // GET: api/Status/5
        [HttpGet("{id}", Name = "Get")]
        [Authorize]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Status
        [HttpPost]
        [Authorize]
        public Status Post([FromBody] Status value)
        {
            dbContext.Statuses.Add(value);
            dbContext.SaveChanges();
            return value;
        }

        // PUT: api/Status/5
        [HttpPut("{id}")]
        [Authorize]
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
