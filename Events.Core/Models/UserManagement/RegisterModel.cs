using Events.Core.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace Events.Api.Models.UserManagement
{
    public class RegisterModel : Model
    {
 
        [Required(ErrorMessage ="User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }

        public long sectionId { get; set; }
        public bool isEnabled { get; set; }
        public bool isHead { get; set; }
        public long DepartmentId { get; set; }

        public string Section { get; set; }

        public string Department { get; set; }

        [Required(ErrorMessage = "RoleId is required")]
        public string RoleId { set; get; }

        public string Role { set; get; }


        public long Id { set; get; }

        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }
    }
}



/*The register model defines the parameters for incoming POST requests to the /users/register route of the api,
 * it is set as the parameter to the Register method of the UsersController. When an HTTP POST request is received 
 * to the route, the data from the body is bound to an instance of the RegisterModel, validated and passed to the method.

ASP.NET Core Data Annotations are used to automatically handle model validation, the [Required] attribute is used 
to mark all fields (first name, last name, username & password) as required so if any are missing a validation error 
message is returned from the api.
*/
