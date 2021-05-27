using Events.Api.Models.General;
using Events.Core.Models;

namespace Events.Api.Models.APTs
{
    public class CompanyName : Model
    {
       
   
        public string Name { set; get; }
        public Status Status { set; get; }
    }
}