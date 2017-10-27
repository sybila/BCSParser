using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpExperimentParameterValue
    {
        public int Id { get; set; }
        public string ValueText { get; set; }
        public double? ValueNum { get; set; }
        public DateTime? ValueDate { get; set; }
        public int? ExperimentParameterId { get; set; }
        public int? ExperimentId { get; set; }
    }
}
