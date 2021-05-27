using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Api.Models.Tasks;
using Events.Api.Models.UserManagement;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.Tasks;
using Events.Data;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagment.services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventsServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly AppDbContext context;
        private readonly DbServiceImpl<Incident, IncidentView> incidentService;
        private readonly DbServiceImpl<Task, Taskview> taskService;
        private readonly DbServiceImpl<Status, Status> statusService;
        private readonly IUserService usersService;
        private readonly DbServiceImpl<Organization, Organization> orgService;
        public ValuesController(DbServiceImpl<Incident, IncidentView> svc,
             AppDbContext ctx,
        DbServiceImpl<Status, Status> ss,
            IUserService us,
            DbServiceImpl<Organization, Organization> os,
            DbServiceImpl<Task, Taskview> ts)
        {
            incidentService = svc;
            taskService = ts;
            statusService = ss;
            usersService = us;
            orgService = os;
            context = ctx;
        }

        public bool isOman(string country)
        {
            var c = country.ToLower();
            return c.Equals("om") || c.Equals("oman");
        }

        // GET: api/<ValuesController>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            string username = (string)HttpContext.Items[Constants.UserId.ToString()];
            var user = context.Users.Where(u => u.UserName == username).SingleOrDefault();

            // incidetns
            var incidents = incidentService.GetCount(x => x.statusId == NOTIFICATION.INCIDENT);
            

            // Notification
            long notifications = incidentService.GetCount(x => x.statusId == NOTIFICATION.OPEN_NOTIFICATION);


            // pending ips requests..
            var ipsList = context.IpAddress
                    .Include(x => x.Source)
                    .Include(x => x.Dest)
                    .Where(x => x.IsRequestVarify && !x.IsHandeled)
                    .ToList();
            long ips = ipsList.Where(x => isOman(x.SourceCountry) || isOman(x.DestinationCountry)).ToList().Count();

            var inbox = taskService.GetCount(x => x.statusId == TASK.OPEN && x.asignedforid == user.Id);
            
            var inboxOnProgress = taskService.GetCount(x => x.asignedforid == user.Id && x.statusId == TASK.IN_PROGRESS);

            CountModel count = new CountModel();

            //incidents
            count.incidents = incidents;
            count.notifications = notifications;

            //notifications

            //ips
            count.ips = ips;

            //inbox
            count.inbox = inbox;
            count.inboxOnProgress = inboxOnProgress;
            

            try
            {
                return Ok(SuccessResponse<CountModel>.build(count, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

            //List<Sector> sectors = new List<Sector>();

            //List<Organization> organizations = new List<Organization>();

            //int counter = 0;
            //string line;
            //List<Sector> sectors1 = new List<Sector>();


            //System.IO.StreamReader file =
            //         new System.IO.StreamReader(@"C:\Users\Hamed Al_maskari\Documents\eventsserver\EventsServerSide\EventsServerSide\Controllers\TextFile.txt");
            //while ((line = file.ReadLine()) != null)
            //{
            //    if (counter != 0)
            //    {


            //        if (counter == 1)
            //            sectors1 = context.sectors.ToList();


            //        string[] orgs = line.Split(",");
            //        for (int i = 0; i < orgs.Length; i++)
            //        {
            //            if (!orgs[i].Equals(""))
            //            {
            //                Organization organization = new Organization();
            //                organization.orgname = orgs[i];
            //                organization.SectorId = sectors1[i].id;
            //                organizations.Add(organization);
            //            }


            //        }

            //        context.organizations.AddRange(organizations);
            //        context.SaveChanges();
            //        organizations = new List<Organization>();


            //    }
            //    else
            //    { saveSectors(line); }

            //    counter++;


            //}

            //file.Close();


            //return "value";
        }

        private void saveSectors(string line)
        {
            string[] names = line.Split(",");
            foreach (string n in names)
            {
                Sector sector = new Sector();
                sector.Name = n;
                context.Sectors.Add(sector);
                context.SaveChanges();
            }


        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
