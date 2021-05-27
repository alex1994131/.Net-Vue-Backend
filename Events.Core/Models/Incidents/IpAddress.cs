using Events.Core.Models;
using Events.Core.Models.Incidents;
using System;

namespace Events.Api.Models.Incidents
{
    public class IpAddress : Model
    {
        public Ip Source { set; get; }
        public Ip Dest { set; get; }
        public DateTime BeginTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public string IntrusionSet { get; set; }
        public string SourceCountry { get; set; }
        public string DestinationCountry { get; set; }
        public string DataLength { get; set; }
        public string SignatureTitle { get; set; }
        public string SignatureContent { get; set; }
        public string SignatureClassification { get; set; }
        public string TotalHits { get; set; }
        public string AptGroup { get; set; }
        public OwnerDetails OwnerDetail { get; set; }
        public bool IsHandeled { get; set; }
        public bool IsKnown { get; set; }
        public bool IsRequestVarify { get; set; }
    }
}