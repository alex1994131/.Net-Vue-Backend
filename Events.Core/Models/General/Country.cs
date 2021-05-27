using Events.Api.Models.APTs;
using Events.Core.Models;
using Events.Core.Models.APTs;
using System.Collections.Generic;

namespace Events.Api.Models.General
{
    public class Country : Model
    {


        public string CountryName { set; get; }
        public string Contenant { set; get; }
        public IList<TargetedCountry> Targeted { set; get; }
        public IList<OriginCountry> Origin { set; get; }

    }
}