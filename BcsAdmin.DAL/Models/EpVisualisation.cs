using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpVisualisation
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string VisualisationXml { get; set; }
        public int? Active { get; set; }
    }
}
