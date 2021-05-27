using Events.Api.Authorization;
using Events.Api.Logging;
using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Api.Models.Tasks;
using Events.Api.Models.UserManagement;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.Logging;
using Events.Data;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagment.services;

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {

        private IServiceFactory ServiceFactory;
        private IUserService usersService;
        private readonly IWebHostEnvironment host;
        public IncidentController(IServiceFactory service, IUserService us, IWebHostEnvironment hostingEnvironment)
        {
            ServiceFactory = service;
            host = hostingEnvironment;
            usersService = us;
        }

        [HttpGet("getIncidentById")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.NOTIFICATIONS, Value: VALUES.VIEW)]
        public Incident GetIncidentById(int id)
        => ServiceFactory.ServicOf<Incident, IncidentView>().Find(id);


        // GET: api/Incident
        [HttpGet("incidents")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.NOTIFICATIONS, Value: VALUES.VIEW)]
        public List<IncidentView> GetIncidents()
        => ServiceFactory.ServicOf<Incident, IncidentView>().GetItems(x => x.statusId == NOTIFICATION.INCIDENT);




        [HttpGet("closed_incidents")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.NOTIFICATIONS, Value: VALUES.VIEW)]
        public List<IncidentView> GetClosedIncidents()
        => ServiceFactory.ServicOf<Incident, IncidentView>().GetItems(x => x.statusId == NOTIFICATION.CLOSED_INCIDENT);



        [HttpGet("notifications")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.NOTIFICATIONS, Value: VALUES.VIEW)]
        public List<IncidentView> GetNotification()
        => ServiceFactory.ServicOf<Incident,IncidentView>().GetItems(x => x.statusId == NOTIFICATION.OPEN_NOTIFICATION);

        [HttpGet("UserIncidentsRequests")]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public IActionResult UserIncidentsRequests()
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = usersService.GetByUsername(username);
                var assignments = ServiceFactory.ServicOf<EntityAssignment, EntityAssignment>().GetItems(x => x.User.Id == user.Id && !x.IsHandeled).Select(x => x.IncidentId).ToList();
                var list = ServiceFactory.ServicOf<Incident, IncidentView>().GetItems(x => (x.createdById == user.Id && x.statusId == NOTIFICATION.EDIT_INCIDENT) || assignments.Any(v => v == x.id) );
                return Ok(SuccessResponse<IncidentView>.build(null, 0,list)); ;
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
        

        [HttpGet("handledNotifications")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.NOTIFICATIONS, Value: VALUES.VIEW)]
        public List<IncidentView> GetIgnoredNotification()
        => ServiceFactory.ServicOf<Incident, IncidentView>().GetItems(x => x.statusId == NOTIFICATION.IGNORED_NOTIFICATION || x.statusId == NOTIFICATION.CLOSED_NOTIFICATION,30);


        [HttpGet("closed_notification")]
        [ServiceFilter(typeof(LogAction))]
        [ClaimsAuth(Type: TYPES.NOTIFICATIONS, Value: VALUES.VIEW)]
        public List<IncidentView> GetClosedNotification()
        => ServiceFactory.ServicOf<Incident, IncidentView>().GetItems(x => x.statusId == NOTIFICATION.CLOSED_NOTIFICATION);


        [HttpGet("incidentData")]
        public Object GetData()
        {
            return new
            {
                Saverity = ServiceFactory.GetConstants(x => x.Saverities.ToList()),
                Category = ServiceFactory.GetConstants(x => x.Categories.ToList()),
                Urgancey = ServiceFactory.GetConstants(x => x.Urganceys.ToList()),
            };
        }

        // GET: api/Incident/5
        [HttpGet("{id}")]
        [Authorize]
        public Incident Get(int id)
        {
            return ServiceFactory.ServicOf<Incident, IncidentView>().Find(id);
        }

        [HttpGet("GetComments")]
        [Authorize]
        public List<IncidentComment> GetComments(int incident)
        {

            Incident inc = ServiceFactory.ServicOf<Incident, IncidentView>().Find(incident);
            if (inc.Comments == null)
                return new List<IncidentComment>();
            return inc.Comments.ToList();
        }


        [HttpGet("incidentsByTask")]
        [Authorize]
        public IActionResult getRelatedIncidents(int id)
        {
            try { return Ok(SuccessResponse<Incident>.build(ServiceFactory.ServicOf<Incident, IncidentView>().Find(id), 0, null)); }
            catch (Exception e){ return Ok(FailedResponse.Build(e.Message));}
        }


        // POST: api/Incident
        [HttpPost]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Incident value)
        {
            try
            {
      
                foreach (var att in value.Attachments)
                {

                    string v = ServiceFactory.ServicOf<Incident, IncidentView>().UploadFile(att.Attachment, host);
                    att.Attachment.Is64base = false;
                    att.Attachment.Url = v;
                    att.Attachment.Content = null;
                }

                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = usersService.GetByUsername(username);
                var status = ServiceFactory.ServicOf<Status, Status>().Find(s => s.Id == NOTIFICATION.OPEN_NOTIFICATION);
                EntityStatus estatus = new EntityStatus() { Status = status };
                value.CreatedBy = user;
                value.Status = estatus;
                value.Saverity = ServiceFactory.GetConstant(x => x.Saverities.Find(value.Saverity.Id));
                value.Category = ServiceFactory.GetConstant(x => x.Categories.Find(value.Category.Id));
                value.Urgancey = ServiceFactory.GetConstant(x => x.Urganceys.Find(value.Urgancey.Id));
                if (value.Orgs.Count > 0){value.Orgs = value.Orgs.Select(org => new OrgsIncidentRel {Organization = ServiceFactory.ServicOf<Organization, Organization>().Find(org.Organization.Id) }).ToList();}
                ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(value, user,new[] { new Change() { OldValue = "قيد  الإنشاء", newValue = "تم الإنشاء", Field = "الحالة" } }.ToList());
                Incident incident = await ServiceFactory.ServicOf<Incident, IncidentView>().AddItem(value);
                await ServiceFactory.NotificationHelper().BuildNotification(user,status,(int)EntityType.Incident,incident.Id);

                return Ok(SuccessResponse<Incident>.build(incident, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }
        
        
        
        [HttpPost("Search")]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public IActionResult Search([FromBody] SearchModel value)
        {
            try
            {
                List<IncidentView> incidents = ServiceFactory.ServicOf<Incident, IncidentView>()
                    .GetItems(v => v.Date >= value.fromDate && v.Date <= value.toDate && 
                    v.statusId != NOTIFICATION.EDIT_INCIDENT)
                    .Where(x => isMatch(value,x)).ToList();
                return Ok(SuccessResponse<IncidentView>.build(null, 0, incidents));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        private bool isMatch(SearchModel value ,IncidentView v)
        {
            List<long> statusList = !value.status.Equals(String.Empty) ?
                value.status.Split(',').Select(x=> Int64.Parse(x)).ToList() : new List<long>();
            bool status = true;
            if (statusList.Count > 0)
                status = statusList.IndexOf(v.statusId) != -1;
            if (!value.key.Equals(String.Empty)) 
                status = v.subject.Contains(value.key) || v.OrgName.Contains(value.key);
            
            return status;
        }


        [HttpPost("comments")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] Incident value)
        {
            try
            {

                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                Incident incident = ServiceFactory.ServicOf<Incident, IncidentView>().Find(value.Id);
                var status = ServiceFactory.ServicOf<Status, Status>().Find(s => s.Id == NOTIFICATION.ADD_COMMENT);
                IncidentComment incidentComment = value.Comments[0];
                incidentComment.Comment.CreatedDate = DateTime.Now;
                EUser eUser = usersService.GetByUsername(username);
                incidentComment.Comment.CreatedBy = eUser;
                if (incident.Comments == null) { incident.Comments = new List<IncidentComment>(); }
                incident.Comments.Add(incidentComment);
                ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(incident, eUser,new[] { new Change() { OldValue = "إضافة تعليق", newValue = "إضافة تعليق", Field = "التعليقات" } }.ToList());
                Incident newIncident = await ServiceFactory.ServicOf<Incident, IncidentView>().UpdateEntity(incident);
                var lastCommentId = newIncident.Comments[newIncident.Comments.Count - 1].CommentId;
                await ServiceFactory.NotificationHelper().BuildNotification(eUser,status,(int)EntityType.Comment,lastCommentId, (int)EntityType.Incident,incident.Id);

                return Ok(SuccessResponse<Incident>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        

        [HttpPost("IncidentAssignmentResponse")]
        [Authorize]
        public async Task<IActionResult> IncidentAssignmentResponse([FromBody] AssignModelIncident value)
        {
            try
            {
                DbServiceImpl<Incident, IncidentView> service = ServiceFactory.ServicOf<Incident, IncidentView>();
                Status status = ServiceFactory.ServicOf<Status, Status>().Find(NOTIFICATION.REQUEST_RESPONSE);
                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                EUser eUser = usersService.GetByUsername(username);
                var Incident = service.Find(value.Incident);
                foreach (EntityAssignment assignment in Incident.Assignments) {
                    if (assignment.Id == value.AssignmentId)
                    {
                        assignment.Response = value.Response;
                        assignment.IsHandeled = true;                        
                    }
                }
                
                ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(Incident, eUser, new[] { new Change() { OldValue = "إضافة قسيمة إجراء", newValue = "إضافة قسيمة إجراء", Field = "قسيمة إجراء" } }.ToList());
                await service.UpdateEntity(Incident);
                await ServiceFactory.NotificationHelper().BuildNotification(eUser, status, (int)EntityType.Incident, Incident.Id);

                return Ok(SuccessResponse<Incident>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpPost("addUsersToAssignment")]
        [Authorize]
        public async Task<IActionResult> addUsersToInciednt([FromBody] AssignModelIncident value)
        {
            try
            {
                DbServiceImpl<Incident, IncidentView> service = ServiceFactory.ServicOf<Incident, IncidentView>();
                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                EUser eUser = usersService.GetByUsername(username);
                var Incident = service.Find(value.Incident);
                List<EUser> Users = value.Users.Select(x => usersService.GetById(x)).ToList();
                Status status = ServiceFactory.ServicOf<Status, Status>().Find(NOTIFICATION.ASSIGN_USER);
                string usersString = string.Join(",", Users.Select(x => x.FullName).ToList());
                List<EntityAssignment> assignments = Users.Select(x => new EntityAssignment() { CreatedDate = DateTime.Now, Request = value.Request, User =x,CreatedBy = eUser,Status = Incident.Status } ).ToList();
                ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(Incident, eUser, new[] { new Change() { OldValue = "إضافة موظف", newValue = "إضافة موظف", Field = " إضافة" + usersString } }.ToList());
                Incident.Assignments.AddRange(assignments);
                await service.UpdateEntity(Incident);
                await ServiceFactory.NotificationHelper().BuildNotificationForUsers(eUser, Users, status, (int)EntityType.Incident, Incident.Id);
                
                return Ok(SuccessResponse<Incident>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("addReplay")]
        [Authorize]
        public async Task<IActionResult> addReplay([FromBody] Comment value)
        {
            try
            {
                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                EUser eUser = usersService.GetByUsername(username);
                var status = ServiceFactory.ServicOf<Status, Status>().Find(s => s.Id == NOTIFICATION.ADD_COMMENT);
                Comment parentComment = ServiceFactory.ServicOf<Comment, Comment>().Find(value.Id);
                Comment reply = value.Replaies[value.Replaies.Count - 1];
                reply.CreatedBy = eUser;
                reply.CreatedDate = DateTime.Now;
                parentComment.Replaies.Add(reply);
                await ServiceFactory.ServicOf<Comment, Comment>().UpdateEntity(parentComment);
                var parentId = parentComment.IncidentComments[0].IncidentId; 
                await ServiceFactory.NotificationHelper().BuildNotification(eUser, status, (int)EntityType.Comment, parentComment.Id, (int)EntityType.Incident, parentId);
                return Ok(SuccessResponse<Incident>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpPost("closeIncident")]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]

        public async Task<IActionResult> closeIncident([FromBody] CloseReport report)
        {
            try
            {

                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = usersService.GetByUsername(username);
                var entity = ServiceFactory.ServicOf<Incident, IncidentView>().Find(report.reportId);
                var newState = ServiceFactory.ServicOf<Status, Status>().Find(s => s.Id == NOTIFICATION.CLOSED_INCIDENT);
                report.CreatedBy = user;
                report.CreatedDate = DateTime.Now;
                entity.CloseReport = report;
                entity.Status = new EntityStatus() { Status = newState };
                ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(entity, user, new[] { new Change() { OldValue = "حادثة قيد الإجراء", newValue = "حادثة مغلقة", Field = "الحالة" }, new Change() { OldValue = "لا يوجد", newValue = "إضافة تقرير الإغلاق", Field = "تقرير إغلاق" } }.ToList());
                await ServiceFactory.ServicOf<Incident, IncidentView>().UpdateEntity(entity);
                await ServiceFactory.NotificationHelper().BuildNotification(user,newState,(int)EntityType.Incident, entity.Id);
                return Ok(SuccessResponse<Incident>.build(null, 0, null));
            }
            catch (Exception e) { return Ok(FailedResponse.Build(e.Message)); }
            
        }
        [HttpPost("changeIncidentStatus")]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public async Task<IActionResult> changeStatus([FromBody] ChangesModel value)
        {

            try
            {
                var user = usersService.GetByUsername((string)HttpContext.Items[Constants.UserId.ToString()]);
                Status status = ServiceFactory.ServicOf<Status, Status>().Find(s => s.Id == value.changeType);
                var incident = ServiceFactory.ServicOf<Incident, IncidentView>().Find(value.id);
                incident.Status = new EntityStatus() { Status = status };
                incident.ExtraNote1 = value.reasonString;
                if (value.changeType == NOTIFICATION.INCIDENT) // if you change this value , please consider changing it on the client side also
                    ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(incident, user,new[] { new Change() { OldValue = "تنبيه", newValue = "تحويل إلى حادث", Field = "نوعية التنبيه" } }.ToList());
                else if (value.changeType == NOTIFICATION.IGNORED_NOTIFICATION)
                    ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(incident, user,new[] { new Change() { OldValue = "لا يوجد", newValue = value.reasonString, Field = "ملاحظات" },new Change() { OldValue = "تنبيه", newValue = "تنبيه متجاهل", Field = "نوعية التنبيه" }}.ToList());
                else if (value.changeType == NOTIFICATION.CLOSED_NOTIFICATION)
                    ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(incident, user,new[] { new Change() { OldValue = "لا يوجد", newValue = value.reasonString, Field = "ملاحظات" },new Change() { OldValue = "تنبيه", newValue = "تنبيه مغلق", Field = "نوعية التنبيه" }}.ToList());

                await ServiceFactory.NotificationHelper().BuildNotification(user, status, (int)EntityType.Incident, incident.Id);
                await ServiceFactory.ServicOf<Incident, IncidentView>().UpdateEntity(incident);
                return Ok(SuccessResponse<Incident>.build(null, 0, null));
            }
            catch (Exception e){ return Ok(FailedResponse.Build(e.Message));}

        }

        [HttpPost("category")]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            try
            {
                ServiceFactory.AddConstant(category);
                return Ok(SuccessResponse<Category>.build(null, 0, ServiceFactory.GetConstants(x => x.Categories.ToList())));
            }
            catch (Exception e) { return Ok(FailedResponse.Build(e.Message));}
        }

        [HttpGet("categories")]
        public IEnumerable<Category> All() => ServiceFactory.GetConstants(x => x.Categories.ToList());



        [HttpPost("EditRequest")]
        [ServiceFilter(typeof(LogAction))]
        [Authorize]
        public async Task<IActionResult> EditRequest([FromBody] Incident newValu)
        {
            try
            {
                Incident oldValue = ServiceFactory.ServicOf<Incident, IncidentView>().Find(newValu.Id);

                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = usersService.GetByUsername(username);
                var status = ServiceFactory.ServicOf<Status, Status>().Find(s => s.Id == NOTIFICATION.OPEN_NOTIFICATION);


                oldValue.Subject = newValu.Subject;
                oldValue.Description = newValu.Description;
                oldValue.Date = newValu.Date;
                oldValue.Time = newValu.Time;
                oldValue.Status = new EntityStatus() { Status = status };
                oldValue.Signature = newValu.Signature;
                oldValue.Saverity = ServiceFactory.GetConstant(x => x.Saverities.Find(newValu.Saverity.Id));
                oldValue.Category = ServiceFactory.GetConstant(x => x.Categories.Find(newValu.Category.Id));
                oldValue.Urgancey = ServiceFactory.GetConstant(x => x.Urganceys.Find(newValu.Urgancey.Id));
                ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(oldValue, user, new[] { new Change() { OldValue = "إنتظار التعديل", newValue ="تم التعديل", Field = "الحالة" }}.ToList());
                if (oldValue.Orgs.Count > 0)oldValue.Orgs = newValu.Orgs.Select(org => new OrgsIncidentRel { Organization = ServiceFactory.ServicOf<Organization, Organization>().Find(org.Organization.Id) }).ToList();
                Incident incident = await ServiceFactory.ServicOf<Incident, IncidentView>().UpdateEntity(oldValue);
                await ServiceFactory.NotificationHelper().BuildNotification(user, status, (int)EntityType.Incident, incident.Id);
                return Ok(SuccessResponse<Incident>.build(incident, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
    }

}
