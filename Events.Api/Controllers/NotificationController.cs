using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events.Api.Models.General;
using Events.Core.Models;
using Events.Core.Models.General;
using Events.Core.Models.Notifications;
using Events.Service.Service;
using Microsoft.AspNetCore.Mvc;
using UserManagment.services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Events.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private IServiceFactory ServiceFactory;
        private IUserService UsersService;
        private Dictionary<long, String> titles = new Dictionary<long, string>() {
            {NOTIFICATION.CLOSED_INCIDENT,"تم غلق الحادثة: " },
            {NOTIFICATION.OPEN_NOTIFICATION,"تم تسجيل تنبيه : " },
            {NOTIFICATION.EDIT_INCIDENT,"طلب تعديل التنبيه: " },
            {NOTIFICATION.IGNORED_NOTIFICATION,"تم تجاهل التنبيه: " },
            {NOTIFICATION.INCIDENT,"تم تحويل التنبيه إلى حادثة: " },
            {NOTIFICATION.ADD_COMMENT,"تم التعليق على التنبيه: " },
            {NOTIFICATION.ASSIGN_USER,"تم إضافتك ف التنبيه: " },
            {NOTIFICATION.REQUEST_RESPONSE,"تم الرد على قسيمة إجراء  " },
            {TASK.ADD_COMMENT,"تم التعليق على المهمة: " },
            {TASK.OPEN,"تم إسناد مهمة: " },


        };
        public NotificationController(IServiceFactory service, IUserService us)
        {
            ServiceFactory = service;
            UsersService = us;
        }

        // GET: api/<NotificationController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                string username = (string)HttpContext.Items[Constants.UserId.ToString()];
                var user = UsersService.GetByUsername(username);
                List<NotificationView> list = ServiceFactory.ServicOf<Notification, NotificationView>().GetItems(x => x.OwnerId == user.Id,20).Select(SetProperTitle).ToList();
                return Ok(SuccessResponse<NotificationView>.build(null, 0, list));
            }
            catch (Exception e)
            {
                return Ok(FailedResponse.Build(e.Message));
            }
        }

        [HttpPost("updateStatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] Notification value)
        {
            try {
                Notification oldValue = ServiceFactory.ServicOf<Notification, NotificationView>().Find(value.Id);
                oldValue.NotificationOwners.ForEach(x => {
                    if (x.Id == value.NotificationOwners[0].Id)
                        x.isNew = false;
                });

               await ServiceFactory.ServicOf<Notification, NotificationView>().UpdateEntity(oldValue);
                return Ok(); 

            } catch (Exception e)
            
            {
                return Ok(FailedResponse.Build(e.Message)); 
            }

        }

        private NotificationView SetProperTitle(NotificationView note, int arg2)
        {

            note.Title = titles[note.StatusId] + note.EntityTitle;
            return note;
        }

    }
}
