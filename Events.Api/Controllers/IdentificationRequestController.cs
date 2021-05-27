
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using Events.Api.Models.Incidents;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.IpIdentification;
using Events.Core.Models.Logging;
using Events.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentificationRequestController : ControllerBase
    {
        private readonly AppDbContext context;
        public IdentificationRequestController(AppDbContext impl)
        {
            context = impl;
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                Incident inc = context.Incidents
                   .Where(x => x.Id == id)
                   .Include(x => x.IpAddresses).ThenInclude(x => x.Source)
                    .Include(x => x.IpAddresses).ThenInclude(x => x.Dest)
                    .Include(x => x.IpAddresses).ThenInclude(x => x.OwnerDetail)
                   .SingleOrDefault();
                return Ok(SuccessResponse<IpAddress>.build(null, 0, inc.IpAddresses));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("getOwnerId")]
        public IActionResult Get(string id)
        {
            try
            {
                OwnerDetails details = context.OwnerDetails.Where(x => x.subsId == id).SingleOrDefault();

                return Ok(SuccessResponse<OwnerDetails>.build(details, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet]
        [Authorize]

        public IActionResult Get()
        {
            try
            {
                List<IpAddress> list = context.IpAddress
                    .Include(x => x.Source)
                    .Include(x => x.Dest)
                    .Include(x => x.OwnerDetail)
                    .ToList();

                list = list.Where(x => isOman(x.SourceCountry) || isOman(x.DestinationCountry)).ToList();

                return Ok(SuccessResponse<IpAddress>.build(null, 0, list));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }
        [HttpGet("getIncidentsWithIdRequests")]
        [Authorize]
        public IActionResult GetIncidentsById()
        {
            try
            {
                List<Incident> list = context.Incidents
                    .Where(x => x.IsIpsIdentificationRequested)
                    .ToList();

                return Ok(SuccessResponse<Incident>.build(null, 0, list));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        public bool isOman(string country)
        {
            var c = country.ToLower();
            return c.Equals("om") || c.Equals("oman");
        }

        // POST api/<RequestIPController>
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] IdentificationRequest value)
        {
            try
            {
                var user = context.Users.Where(u => u.UserName ==
                (string)HttpContext.Items[Constants.UserId.ToString()]).SingleOrDefault();
                Incident incident = context.Incidents
                    .Where(x => x.Id == value.id).Include(x => x.Changes).Include(x => x.IpAddresses).SingleOrDefault();
                incident.IpAddresses
                .ForEach(ip =>
                {
                    ip.IsRequestVarify = true;
                    context.IpAddress.Update(ip);
                });
                ChangeLog changeLog = ChangeLog.Build(user);
                changeLog.addChangeField(new ChangeLogField()
                {
                    FieldName = "طلب تعريف العناوين",
                    OldValue = "لا يوجد",
                    NewValue = "إرسال طلب تعريف"
                });

                incident.Changes.Add(changeLog);
                incident.IsIpsIdentificationRequested = true;
                context.SaveChanges();
                return Ok(SuccessResponse<Incident>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        // PUT api/<RequestIPController>/5
        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] IdentificationRequest value)
        {
            try
            {
                IpAddress ip = context.IpAddress.Find(value.id);
                OwnerDetails details = context.OwnerDetails.Where(x => x.subsId == value.subsId).SingleOrDefault();
                if (details != null)
                {
                    ip.OwnerDetail = details;
                }
                else
                {
                    ip.OwnerDetail = new OwnerDetails();
                    ip.OwnerDetail.cid = value.cid;
                    ip.OwnerDetail.subsId = value.subsId;
                    ip.OwnerDetail.ownerSub = value.ownerSub;
                    ip.OwnerDetail.ownerType = value.ownerType;
                    ip.OwnerDetail.phoneNum = value.phoneNum;
                    ip.AptGroup = value.aptGroup;
                }

                ip.IsHandeled = true;
                ip.IsKnown = value.isKnown;
                context.IpAddress.Update(ip);
                context.SaveChanges();
                return Ok(SuccessResponse<Incident>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        // DELETE api/<RequestIPController>/5
        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int id)
        {
            return Ok(SuccessResponse<Incident>.build(null, 0, null));
        }
    }
}
