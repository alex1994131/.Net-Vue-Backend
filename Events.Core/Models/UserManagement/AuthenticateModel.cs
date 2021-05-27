using System.ComponentModel.DataAnnotations;

namespace Events.Api.Models.UserManagement
{
    public class AuthenticateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
