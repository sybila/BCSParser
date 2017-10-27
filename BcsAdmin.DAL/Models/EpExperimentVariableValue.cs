using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpExperimentVariableValue
    {
        public int Id { get; set; }
        public double? Time { get; set; }
        public double? Value { get; set; }
        public int? ExperimentVariableId { get; set; }
    }
}
