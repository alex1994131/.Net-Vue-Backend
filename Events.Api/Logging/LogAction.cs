using Events.Core.Models.Logging;
using Events.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Events.Api.Logging
{
    public class LogAction : ActionFilterAttribute
    {

        private readonly AppDbContext context;

        public LogAction(AppDbContext ctx)
        {
            context = ctx;
        }



        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    //OkObjectResult okObjectResult = filterContext.Result as OkObjectResult;
        //    //object v = okObjectResult?.Value.GetType().GetProperty("data").GetValue(okObjectResult.Value);
        //    //Type myType = v.GetType();
        //    //IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
        //    //var val_a = v.GetType().GetProperty("fields").GetValue(v, null);

        //    //var result = filterContext.Result;
          

        //  //  OkObjectResult okObjectResult = filterContext.Result as OkObjectResult;

        //  //  string oldvalue;
        //  //  string newVal;
        //  //  bool success = false;
        //  //  List<ChangeLogField> changes = new List<ChangeLogField>();
        //  //  try
        //  //  {
        //  //      var value = okObjectResult.Value as SuccessACResponse<Incident>;
        //  //      changes = value.fields;
        //  //      success = true;
        //  //  }
        //  //  catch (Exception e) { }
                                                                                                                                                                                                     
            
        //  ////
           
        //  //  //
        //  //  var username = filterContext.HttpContext.User.Identity.Name;
        //  //  ChangeLog audit = new ChangeLog()
        //  //  {

        //  //      changedBy = context.users.Where(x => x.UserName == username).SingleOrDefault(),
        //  //      fields = changes

        //  //  };
        //  //  context.changeLogs.Add(audit);
        //  //  context.SaveChanges();
        //    base.OnActionExecuted(filterContext);


        //}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var username = filterContext.HttpContext.User.Identity.Name;
            UserActivity audit = new UserActivity()
            {
                UserName = context.Users.Where(x => x.UserName == username).SingleOrDefault(),
                IPAddress = filterContext.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString(),
                Url = ((ControllerBase)filterContext.Controller).ControllerContext.ActionDescriptor.ControllerName + "/" + ((ControllerBase)filterContext.Controller).ControllerContext.ActionDescriptor.ActionName,
                Timestamp = DateTime.UtcNow
            };
            context.UserActivity.Add(audit);
            context.SaveChanges();
            base.OnActionExecuting(filterContext);
        }
    }
}
