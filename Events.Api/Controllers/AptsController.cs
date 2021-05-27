using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Events.Api.Authorization;
using Events.Api.Models.APTs;
using Events.Core.Models.APTs;
using Events.Core.Models.General;
using Events.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventsManagemtns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AptsController : ControllerBase
    {

        private readonly AppDbContext _ctx;
        public AptsController(AppDbContext dbContext) {
            _ctx = dbContext;
        }
        // GET: api/<AptsController>
        [HttpGet]
        [Authorize]
        public IActionResult Get() {
            try
            {
                List<APT> lists
                   = _ctx.Apts
                    .Include(x => x.Targeted).ThenInclude(x => x.Country)
                    .Include(x => x.Origin).ThenInclude(x => x.Country)
                    .Include(x => x.ThreatSignatures)
                    .Include(x => x.AttackStratigies)
                    .Include(x => x.AlternativeNames)
                    .Include(x => x.CompanyNames)
                    .Include(x => x.ToolsNames)
                    .Include(x => x.CreatedBy)
                    .Include(x => x.TargetSectorNames).ThenInclude(x => x.Sector)
                    .Include(x => x.Contents).ThenInclude(x => x.CreatedBy)
                    .Include(x => x.attachments).ThenInclude(x => x.Attachment)
                    .ToList();

                return Ok(SuccessResponse<APT>.build(null, 0, lists));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
           

    }

        // POST api/<AptsController>
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] APT apt)
        {

            try
            { 
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = _ctx.Users.Where(u => u.UserName == username).SingleOrDefault();

                apt.Targeted = apt.Targeted.Select(tc => new TargetedCountry() { Country = _ctx.Countries.Where(c => c.Id == tc.Country.Id).SingleOrDefault() }).ToList(); ;
                apt.Origin = apt.Origin.Select(oc => new OriginCountry() { Country = _ctx.Countries.Where(c => c.Id == oc.Country.Id).SingleOrDefault() }).ToList();
                apt.TargetSectorNames = apt.TargetSectorNames.Select(oc => new TargetedSector() { Sector = _ctx.Sectors.Where(c => c.Id == oc.Sector.Id).SingleOrDefault() }).ToList();
                apt.CreatedBy = user;
                apt.Contents.ForEach(x => x.CreatedBy = user);

                _ctx.Apts.Add(apt);
                _ctx.SaveChanges();
                long id = id = _ctx.Incidents.Max(c => c.Id);
            return Ok(SuccessResponse<APT>.build(_ctx.Apts.Find(id), id));
        } catch (Exception e) {
                return Ok(FailedResponse.Build(e.Message));
    }
}

        // PUT api/<AptsController>/5
        [HttpPut]
        [ClaimsAuth(Type: TYPES.APT, Value: VALUES.UPDATE)]
        public void Put([FromBody] APT newAPT)
        {

            // define un-known type thats pointing to apt
            APT aPT = _ctx.Apts.Where(x => x.Id == newAPT.Id)
                .Include(x => x.Targeted)
                .Include(x => x.Origin)
                .Include(x => x.ThreatSignatures)
                .Include(x => x.AttackStratigies)
                .Include(x => x.AlternativeNames)
                .Include(x => x.CompanyNames)
                .Include(x => x.ToolsNames)
                .Include(x => x.TargetSectorNames)
                .Include(x => x.Contents)
                .Include(x => x.attachments)
                .FirstOrDefault();


            List<TargetedCountry> targetedCountries = new List<TargetedCountry>();
            List<OriginCountry> originCountry = new List<OriginCountry>();


          
            _ctx.Entry(aPT).CurrentValues.SetValues(newAPT);
            _ctx.SaveChanges();
        }

        [HttpPost("AddContents")]
        [Authorize]
        public void AddContent([FromBody] APT value)
        {

            String username = (string)HttpContext.Items[Constants.UserId.ToString()];
            APT apt = _ctx.Apts.Find(value.Id);
            Content content = value.Contents[0];
            content.createdDate = DateTime.Now;
            content.CreatedBy = _ctx.Users.Where(u => u.UserName == username).SingleOrDefault();

            if (apt.Contents == null)
                apt.Contents = new List<Content>();
            apt.Contents.Add(content);
            _ctx.Entry(apt).CurrentValues.SetValues(apt);
            _ctx.SaveChanges();
        }

        [HttpGet("GetContents")]
        [Authorize]
        public IActionResult GetContents(int apt)
        {
            try
            {
                APT apt1 = _ctx.Apts.Where(x => x.Id == apt)
                    .Include(x => x.Contents).ThenInclude(x => x.CreatedBy).ToList().FirstOrDefault();

                return Ok(SuccessResponse<APT>.build(apt1,apt,null));
            }
            catch(Exception e)
            {

                return Ok(FailedResponse.Build(e.Message));
            }
        }


    }
}
