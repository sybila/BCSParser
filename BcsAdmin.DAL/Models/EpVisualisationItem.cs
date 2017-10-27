using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpVisualisationItem
    {
        public int Id { get; set; }
        public int? VisualisationId { get; set; }
        public int? ItemId { get; set; }
        public string Type { get; set; }
        public sbyte Nonrecursive { get; set; }
    }
}
