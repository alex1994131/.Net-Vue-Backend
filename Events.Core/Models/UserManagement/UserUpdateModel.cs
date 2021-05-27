using Events.Api.Models.General;
using Events.Core.Models;
using Events.Core.Models.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Events.Core.Models.UserManagement
{
  public  class UserUpdateModel 
    {
        public long id { set; get; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public long sectionId { get; set; }
        public string RoleId { get; set; }

        public bool isEnabled { get; set; }

        public bool isHead { get; set; }

    }
}
