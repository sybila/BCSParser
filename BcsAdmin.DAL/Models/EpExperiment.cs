using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpExperiment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Protocol { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Inserted { get; set; }
        public int? ExperimentSeriesId { get; set; }
        public int? ParentId { get; set; }
        public ExperimentStatus Status { get; set; }
    }
}
