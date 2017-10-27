using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpEntityCompositionInstance
    {
        public int Id { get; set; }
        public int? CompositionId { get; set; }
        public int? EntityId { get; set; }
        public string Type { get; set; }
    }
}
