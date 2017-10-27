using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpExperimentParameterItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ExperimentParameterId { get; set; }
    }
}
