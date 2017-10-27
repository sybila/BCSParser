using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpExperimentToEntity
    {
        public int Id { get; set; }
        public int? ExperimentVariableId { get; set; }
        public int? EntityId { get; set; }
    }
}
