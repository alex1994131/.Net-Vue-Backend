namespace Events.Api.Models.UserManagement
{
    public class UpdateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}



/*The update model defines the parameters for incoming PUT requests to the /users/{id} route of the api, 
 * it is set as the parameter to the Update method of the UsersController. When an HTTP PUT request is received 
 * to the route, the data from the body is bound to an instance of the UpdateModel, validated and passed to the method.

There are no validation attributes used in the update model so all fields are optional and only fields that 
contain a value are updated. The logic for updating users is located in the Update method of the UserService.
*/
