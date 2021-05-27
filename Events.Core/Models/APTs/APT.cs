
using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.APTs;
using Events.Core.Models.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Api.Models.APTs
{
    public class APT : Model
    {

        public string Name { set; get; }

        public List<TargetedCountry> Targeted { set; get; }
        public List<OriginCountry> Origin { set; get; }
        public List<Content> Contents { set; get; }
        public List<ThreatSignature> ThreatSignatures { set; get; }
        public List<AlternativeName> AlternativeNames { set; get; }
        public List<AttackStratigie> AttackStratigies { set; get; }
        public List<AptAttachment> attachments { set; get; }
        public IList<CompanyName> CompanyNames { set; get; }
        public IList<TargetedSector> TargetSectorNames { set; get; }
        public IList<ToolName> ToolsNames { set; get; }
        public long Id { get; set; }
        public EUser CreatedBy { get; set; }
        public EUser LastUpdateBy { get; set; }
        public DateTime CreatedDate { get ; set ; }
        public DateTime CreatedTime { get ; set; }
        public List<ChangeLog> Changes { get ; set; }
    }

 
}