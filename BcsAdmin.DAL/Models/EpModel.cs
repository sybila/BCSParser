using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VisualisationXml { get; set; }
        public int? UnitId { get; set; }
        public int? UserId { get; set; }
        public string Solver { get; set; }
        public string Status { get; set; }
    }
}
