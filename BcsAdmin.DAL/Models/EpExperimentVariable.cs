using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpExperimentVariable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Protocol { get; set; }
        public string Formula { get; set; }
        public int? UnitId { get; set; }
        public int? ExperimentId { get; set; }
        public VariableType Type { get; set; }
    }
}
