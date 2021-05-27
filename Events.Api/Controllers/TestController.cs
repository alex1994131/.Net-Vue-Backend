using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Api.Models.UserManagement;
using Events.Core.Models.Services;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Mvc;
using UserManagment.services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly DbServiceImpl<Incident, EUser> incidentService;
        private readonly DbServiceImpl<Status, String> statusService;
        private readonly IUserService usersService;
        private readonly DbServiceImpl<Organization, String> orgService;
        public TestController(DbServiceImpl<Incident, EUser> svc,
            DbServiceImpl<Status, String> ss,
            IUserService us,
            DbServiceImpl<Organization, String> os)
        {
            incidentService = svc;
            statusService = ss;
            usersService = us;
            orgService = os;
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
