
using System.Collections.Generic;
using System.Linq;
using Events.Api.Models.General;
using Events.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventsManagemtns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusTypesController : ControllerBase
    {

        private readonly AppDbContext context;

        public StatusTypesController(AppDbContext impl) {
            context = impl;        
        }

        // GET: api/<StatusTypesController>
   


        // GET api/<StatusTypesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StatusTypesController>
        [HttpPost]
        [Authorize]

        // PUT api/<StatusTypesController>/5


        // DELETE api/<StatusTypesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
