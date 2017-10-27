using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpReaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Equation { get; set; }
        public string Modifier { get; set; }
        public string VisualisationXml { get; set; }
        public int? Active { get; set; }
        public int? IsValid { get; set; }
    }
}
