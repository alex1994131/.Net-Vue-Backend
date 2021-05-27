namespace Events.Api.Models.UserManagement
{

    // The authenticate response model defines the data returned after successful authentication,
    // it includes basic user details and a JWT access token.
    public class AuthenticateResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(EUser employees, string token)
        {
            Id = employees.Id;
            Name = employees.FullName;
            Username = employees.UserName;
            Token = token;
        }
    }
}
