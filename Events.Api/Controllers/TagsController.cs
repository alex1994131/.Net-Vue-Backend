using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events.Core.Models.General;
using Events.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        public TagsController(AppDbContext dbContext)
        {
            _ctx = dbContext;
        }

        // GET: api/<TagsController>
        [HttpGet("allTags")]
        public IEnumerable<Tag> Get()
        {
            List<Tag> tags = _ctx.Tags.ToList();

            return tags;
        }

        
        // POST api/<TagsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }


        // DELETE api/<TagsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
