using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpExperimentVariableToModelSpecie
    {
        public int Id { get; set; }
        public int? ExperimentVariableId { get; set; }
        public int? ModelSpecieId { get; set; }
    }
}
