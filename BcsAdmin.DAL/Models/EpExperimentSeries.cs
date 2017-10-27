using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpExperimentSeries
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string OverallDesign { get; set; }
        public DateTime? Inserted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? UserId { get; set; }
        public ExperimentStatus Status { get; set; }
    }
}
