namespace Events.Api.Models.UserManagement
{
    /*We can create a class “Response” for returning the response value after user registration and user login. 
     * It will also return error messages, if the request fails.
     */

    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }

    }
}
