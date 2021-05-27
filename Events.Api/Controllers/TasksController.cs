using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Api.Models.Tasks;
using Events.Api.Models.UserManagement;
using Events.Core.Models.Employees;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.Logging;
using Events.Core.Models.Tasks;
using Events.Core.Models.ViewModels;
using Events.Data;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using UserManagment.services;

namespace EventsManagemtns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {

        private readonly DbServiceImpl<Task, Taskview> TaskService;
        private readonly DbServiceImpl<Status, Status> StatusService;
        private readonly IWebHostEnvironment host;
        private readonly IUserService UserService;
        private readonly IServiceFactory ServiceFactory;
        public TasksController(IUserService us, IWebHostEnvironment hostingEnvironment, IServiceFactory serFactory)
        {
            UserService = us;
            host = hostingEnvironment;
            ServiceFactory = serFactory;
            TaskService = serFactory.ServicOf<Task, Taskview>();
            StatusService = ServiceFactory.ServicOf<Status, Status>();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            try
            {
                Task task = TaskService.Find(id);
                return Ok(SuccessResponse<Task>.build(task, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        // GET api/<TasksController>/5
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {

            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);
                var results = TaskService.GetItems(x => x.asignedforid == user.Id && (x.statusId == TASK.IN_PROGRESS
                    || x.statusId == TASK.OPEN));


                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("getClosedTasks")]
        [Authorize]
        public IActionResult getClosedTasks()
        {

            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);
                var results = TaskService.GetItems(x => x.asignedforid == user.Id && x.statusId == TASK.CLOSED);
                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("SaecrhAllTasks")]
        [Authorize]
        public IActionResult GetAllTasks([FromBody] SearchModel value)
        {

            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);
                List<Taskview> tasks = ServiceFactory.ServicOf<Task, Taskview>()
                    .GetItems(v => v.createdDate.Date >= value.fromDate && v.createdDate.Date <= value.toDate)
                    .Where(x => isMatch(value, x)).ToList();
                return Ok(SuccessResponse<Taskview>.build(null, 0, tasks));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpGet("Taskview")]
        [Authorize]
        public IActionResult GetTaskview()
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);
                var results = TaskService.GetItems(x => x.createdById == user.Id && x.statusId != TASK.CLOSED);

                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("fetchAllTasks")]
        [Authorize]
        public IActionResult fetchAllTasks()
        {
            try
            {
                var results = TaskService.GetItems(x => x.statusId != TASK.CLOSED);
                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("TasksByOwner")]
        [Authorize]
        public IActionResult fetchTasksByOwner()
        {
            try
            {
                var results = TaskService.GetItems(x => x.statusId != TASK.CLOSED);
                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }



        [HttpGet("GetTasksByDepartment")]
        [Authorize]
        public IActionResult GetTasksByDepartment()
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);

                if (!user.IsHead && !user.IsSubHead && !user.IsAssignedHead)
                    return Ok(SuccessResponse<Taskview>.build(null, 0, new List<Taskview>()));

                var results = TaskService.GetItems(v => v.assignedForDepartmentId == user.Section.Department.Id || v.createdByDepartmentId == user.Section.Department.Id);
                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpGet("subTasks")]
        [Authorize]
        public IActionResult GetSubTask(long parentTask)
        {

            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);

                if (!user.IsHead && !user.IsSubHead && !user.IsAssignedHead)
                    return Ok(SuccessResponse<Taskview>.build(null, 0, new List<Taskview>()));

                var results = TaskService.GetItems(x => x.parentTaskid == parentTask);
                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpGet("relatedTask")]
        [Authorize]
        public IActionResult getRelatedTask(long incident)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);

                var results = TaskService.GetItems(x => x.parentIncidentid == incident);
                return Ok(SuccessResponse<Taskview>.build(null, 0, results));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        [HttpGet("taskTypes")]
        [Authorize]
        public IActionResult GetTaskType()
        {
            try
            {
                List<TaskType> lists = ServiceFactory.GetConstants(u => u.TaskType.ToList());
                return Ok(SuccessResponse<TaskType>.build(null, 0, lists));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }
        [HttpGet("TaskEmpsRel")]
        [Authorize]
        public IActionResult TaskEmp()
        {
            try
            {
                List<Department> lists = ServiceFactory.GetConstants(u => u.Departments
                 .Include(x => x.Sections).ThenInclude(x => x.Tasks).ThenInclude(x => x.Status)
                 .ToList());


                return Ok(SuccessResponse<Department>.build(null, 0, lists));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }



        [HttpPost("SearchAssignedTasks")]
        [Authorize]
        public IActionResult SearchAssignedTasks([FromBody] SearchModel value)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);
                List<Taskview> tasks = ServiceFactory.ServicOf<Task, Taskview>()
                    .GetItems(v => v.asignedforid == user.Id && v.createdDate.Date >= value.fromDate && v.createdDate.Date <= value.toDate)
                    .Where(x => isMatch(value, x)).ToList();
                return Ok(SuccessResponse<Taskview>.build(null, 0, tasks));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }



        [HttpPost("SearchOwnedTasks")]
        [Authorize]
        public IActionResult SearchTasks([FromBody] SearchModel value)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);
                List<Taskview> tasks = ServiceFactory.ServicOf<Task, Taskview>()
                    .GetItems(v => v.createdById == user.Id && v.createdDate.Date >= value.fromDate
                     && v.createdDate.Date <= value.toDate)
                    .Where(x => isMatch(value, x)).ToList();
                return Ok(SuccessResponse<Taskview>.build(null, 0, tasks));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("SearchDepartmentTasks")]
        [Authorize]
        public IActionResult SearchDepartmentTasks([FromBody] SearchModel value)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);

                List<Taskview> tasks = ServiceFactory.ServicOf<Task, Taskview>()
                    .GetItems(v => v.createdDate >= value.fromDate && v.createdDate <= value.toDate)
                    .Where(v => v.assignedForDepartmentId == user.Section.Department.Id || v.createdByDepartmentId == user.Section.Department.Id)
                    .Where(x => isMatch(value, x)).ToList();
                return Ok(SuccessResponse<Taskview>.build(null, 0, tasks));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("SearchAllTasks")]
        [Authorize]
        public IActionResult SearchAllTasks([FromBody] SearchModel value)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);

                List<Taskview> tasks = ServiceFactory.ServicOf<Task, Taskview>()
                    .GetItems(v => v.createdDate >= value.fromDate && v.createdDate <= value.toDate)
                    .Where(x => isMatch(value, x)).ToList();
                return Ok(SuccessResponse<Taskview>.build(null, 0, tasks));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
        private bool isMatch(SearchModel value, Taskview v)
        {

            List<long> statusList = !value.status.Equals(String.Empty) ?
                value.status.Split(',').Select(x => Int64.Parse(x)).ToList() : new List<long>();

            List<long> employees = value.employees != null && !value.employees.Equals(String.Empty) ?
                           value.status.Split(',').Select(x => Int64.Parse(x)).ToList() : new List<long>();



            bool status = true;
            if (statusList.Count > 0)
                status = statusList.IndexOf(v.statusId) != -1;

            if (!status)
                return status;

            if (employees.Count > 0)
                status = employees.IndexOf(v.asignedforid) != -1;

            if (!status)
                return status;

            if (!value.key.Equals(String.Empty))
                status = v.title.Contains(value.key) || v.description.Contains(value.key);

            return status;
        }


        [HttpPost]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> Post([FromBody] Task value)
        {
            try
            {
                Incident incident = new Incident();

                List<TaskAttachments> attachments = value.Attachments;
                foreach (var att in attachments)
                {

                    string v = TaskService.UploadFile(att.attachment, host);
                    att.attachment.Is64base = false;
                    att.attachment.Url = v;
                    att.attachment.Content = null;
                }

                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);
                value.CreatedBy = user;
                value.CreatedDate = DateTime.Now.Date;
                value.TaskType = ServiceFactory.GetConstant(x => x.TaskType.Find(value.TaskType.Id));
                value.Status = ServiceFactory.ServicOf<Status, Status>().Find(s => s.Id == TASK.OPEN);
                if (value.ParentTask.Id != 0)
                {
                    Task task = TaskService.Find(value.ParentTask.Id);
                    value.ParentTask = TaskService.Find(value.ParentTask.Id);
                    ChangeLog changeLog2 = ChangeLog.Build(user);
                    changeLog2.addChangeField(new ChangeLogField()
                    {
                        FieldName = "المهام الفرعية",
                        OldValue = "",
                        NewValue = "إضافة مهمة فرعية"
                    });


                    if (task.Changes == null)
                        task.Changes = new List<ChangeLog>();
                }
                else
                {
                    value.ParentTask = null;
                }

                if (value.ParentIncident.Id != 0)
                {
                    incident = ServiceFactory.ServicOf<Incident, IncidentView>().Find(value.ParentIncident.Id);
                    value.ParentIncident = incident;
                }
                else
                    value.ParentIncident = null;

                ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(value, user, new[] { new Change() { OldValue = "تحت الإنشاء", newValue = "إنشاء مهمة جديد", Field = "الحالة" } }.ToList());
                value.Assigned_for = ServiceFactory.GetConstant<Section>(v => v.Sections.Find(value.Assigned_for.Id));
                var users = UserService.GetAll();
                value.AssignedEmps = value.AssignedEmps.Select(emp => new TaskEmpsRel() { EUser = UserService.GetById(emp.EUser.Id) }).ToList();
                Task task1 = await TaskService.AddItem(value);
                value.AssignedEmps.Select(x => x.EUser);
                await ServiceFactory.NotificationHelper().BuildNotificationForUsers(user, value.AssignedEmps.Select(x => x.EUser).ToList(),
                    value.Status, (int)EntityType.Task, task1.Id);

                if (incident.Id != 0)
                    await ServiceFactory.ChangeLogHelper().AddChangeLogToDb<Incident, IncidentView>(incident, user, new[] { new Change() { OldValue = "إضافة مهمة متعلقة", newValue = "إضافة مهمة متعلقة", Field = "إسناد مهمة" } }.ToList());

                return Ok(SuccessResponse<Task>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }



        [HttpPost("UpdateTask")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> UpdateTask([FromBody] Task newValue)
        {
            try
            {

                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);


                var oldValue = TaskService.Find(newValue.Id);
                oldValue.LastUpdateBy = user;
                oldValue.LastUpdateDate = DateTime.Now.Date;
                foreach (var att in newValue.Attachments)
                {
                    string v = TaskService.UploadFile(att.attachment, host);
                    att.attachment.Is64base = false;
                    att.attachment.Url = v;
                    att.attachment.Content = null;
                    oldValue.Attachments.Add(att);
                }
                oldValue.Description = newValue.Description;
                oldValue.Title = newValue.Title;
                oldValue.Urgent = newValue.Urgent;
                oldValue.Weight = newValue.Weight;
                oldValue.Importance = newValue.Importance;
                oldValue.TaskType = ServiceFactory.GetConstant(x => x.TaskType.Find(newValue.TaskType.Id));
                ServiceFactory.ChangeLogHelper().AddChangeLogToEntity(oldValue, user, new[] { new Change() { OldValue = "قيد التعديل", newValue = "تم التعديل", Field = "الحالة" } }.ToList());
                var users = UserService.GetAll();
                oldValue.AssignedEmps = newValue.AssignedEmps.Select(emp => new TaskEmpsRel() { EUser = UserService.GetById(emp.EUser.Id) }).ToList();
                Task task = await TaskService.UpdateEntity(oldValue);


                return Ok(SuccessResponse<Task>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }


        // PUT api/<TasksController>
        [HttpPut("updateStatus")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> Put(long statusId, long id)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);
                var entity = TaskService.Find(id);
                var st = StatusService.Find(statusId);
                entity.Status = st;
                if (st.Id == TASK.CLOSED)
                    entity.Progress = 100;
                await ServiceFactory.ServicOf<Task, Taskview>().UpdateEntity(entity);
                return Ok(SuccessResponse<Task>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("AddTaskComments")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> AddTaskComments([FromBody] CommentViewModel value)
        {

            try
            {
                List<CommentAttachment> commentAttachments = new List<CommentAttachment>();
                List<Attachment> attachments = value.attachments;
                foreach (var att in attachments)
                {

                    string v = TaskService.UploadFile(att, host);
                    att.Is64base = false;
                    att.Url = v;
                    att.Content = null;
                    commentAttachments.Add(new CommentAttachment()
                    {
                        attachment = att
                    });

                }
                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                Task task = TaskService.Find(value.relid);
                TaskComment comment = new TaskComment();
                comment.Comment = new Comment();
                comment.Comment.Attachments = commentAttachments;
                comment.Comment.CommentString = value.commentString;
                comment.Comment.CreatedDate = DateTime.Now;
                comment.Comment.CreatedBy = UserService.GetByUsername(username);

                if (task.TaskComments == null)
                    task.TaskComments = new List<TaskComment>();
                task.TaskComments.Add(comment);
                await TaskService.UpdateEntity(task);
                return Ok(SuccessResponse<Task>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }


        [HttpPost("addReplay")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> addReplay([FromBody] Comment value)
        {
            try
            {

                String username = (string)HttpContext.Items[Constants.UserId.ToString()];
                Comment parentComment = ServiceFactory.ServicOf<Comment, Comment>().Find(value.Id);
                Comment reply = value.Replaies[value.Replaies.Count - 1];
                reply.CreatedBy = UserService.GetByUsername(username);
                reply.CreatedDate = DateTime.Now;

                parentComment.Replaies.Add(reply);
                await ServiceFactory.ServicOf<Comment, Comment>().UpdateEntity(parentComment);
                return Ok(SuccessResponse<Task>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        [HttpGet("GetTaskComments")]
        [Authorize]
        public IActionResult GetTaskComments(long taskId)
        {
            try
            {
                Task task = TaskService.Find(taskId);
                return Ok(SuccessResponse<Task>.build(task, taskId, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }



        [HttpPut("progress")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> updateProgress(long id, int prog)
        {
            try
            {
                var entity = TaskService.Find(id);
                entity.Progress = prog;
                await TaskService.UpdateEntity(entity);
                return Ok(SuccessResponse<Task>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }
        [HttpPut("status")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> updateStatus(long id)
        {
            try
            {
                Status status = StatusService.Find((Int64)TASK.IN_PROGRESS);
                var entity = TaskService.Find(id);
                entity.Status = status;
                await TaskService.UpdateEntity(entity);
                return Ok(SuccessResponse<Task>.build(null, 0));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("closeTask")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> Put([FromBody] CloseReport report)
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UserService.GetByUsername(username);
                var entity = TaskService.Find(report.reportId);
                report.CreatedBy = user;
                report.CreatedDate = DateTime.Now;
                entity.ClosingReport = report;
                entity.Progress = 100;
                entity.Status = StatusService.Find(TASK.CLOSED);
                await TaskService.UpdateEntity(entity);
                return Ok(SuccessResponse<Task>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        [HttpPost("shareTask")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> ShareTask([FromBody] Task value)
        {
            try
            {
                var task = TaskService.Find(value.Id);
                var emps = value.AssignedEmps.Select(x => new TaskEmpsRel()
                {
                    EUserId = x.EUser.Id
                }).ToList();

                task.AssignedEmps.AddRange(emps);
                await TaskService.UpdateEntity(task);
                return Ok(SuccessResponse<Task>.build(null, 0, null));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(long id)
        {
            try
            {

                Task task = TaskService.Find(id);
                TaskService.Delete(task);
                return Ok(SuccessResponse<Task>.build(null, 0, null));

            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }

        }
    }
}
