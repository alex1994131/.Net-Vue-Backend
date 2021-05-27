
namespace Events.Core.Models.General
{
    public class FailedResponse
    {
        public string message { get; set; } 
        public int status { get; set; } = (int)ResponseCodes.Exception;

        public bool isFailedResponseObject { get; set; } = true;

        private FailedResponse(string m) { message = m; }
        public static FailedResponse Build(string message) {
            return new FailedResponse(message);
        }
    }
}
