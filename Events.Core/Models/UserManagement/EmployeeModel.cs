namespace Events.Api.Models.UserManagement
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

    }
}



/*The user model is a response model that defines the data returned for GET requests to the /users (get all users)
 * and /users/{id} (get user by id) routes of the api. The GetAll and GetById methods of the UsersController convert
 * user entity data into user model data before returning it in the response in order to prevent some properties from 
 * being returned (e.g. password hashes & salts).
 */
