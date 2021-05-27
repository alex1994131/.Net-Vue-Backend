using Events.Core.Models;

namespace Events.Api.Models.Incidents
{
    public class Category  : Model
    {
        public string code { set; get; }
        public string label { set; get; }

    }
}