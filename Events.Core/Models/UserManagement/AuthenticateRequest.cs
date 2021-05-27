using System.ComponentModel.DataAnnotations;

namespace Events.Api.Models.UserManagement
{
    class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}


/*The authenticate request model defines the parameters for incoming requests to the /users/authenticate route,
 * it is attached to the route as the parameter to the Authenticate action method of the users controller.
 * When an HTTP POST request is received by the route, the data from the body is bound to an instance of the 
 * AuthenticateRequest class, validated and passed to the method.
 */
