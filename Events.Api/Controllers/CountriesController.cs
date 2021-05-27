
using System.Collections.Generic;
using System.Linq;
using Events.Api.Models.General;
using Events.Core.Models.General;
using Events.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventsManagemtns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        public CountriesController(AppDbContext dbContext) {
            _ctx = dbContext;
        }
        // GET: api/<CountriesController>
        [HttpGet]
        [Authorize]
        public IEnumerable<object> Get()
        {
            object v = HttpContext.Items[Constants.UserId.ToString()];
            var enumerable = _ctx.Countries.ToList().Select(x=> new 
            { 
                x.CountryName,
                x.Contenant,
                x.Id
            });

            return enumerable;
        }

        // GET api/<CountriesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CountriesController>
        [HttpPost]
        public void Post([FromBody] List<Country> countries)
        {
            _ctx.Countries.AddRange(countries);
            _ctx.SaveChanges();

        }

        // PUT api/<CountriesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CountriesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
