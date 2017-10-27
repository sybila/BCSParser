using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpExperimentToModel
    {
        public int Id { get; set; }
        public DateTime? Validated { get; set; }
        public int? ValidationUserId { get; set; }
        public int? ModelId { get; set; }
        public int? ExperimentId { get; set; }
    }
}
