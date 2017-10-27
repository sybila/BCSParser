using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpExperimentNote
    {
        public int Id { get; set; }
        public double? Time { get; set; }
        public string Note { get; set; }
        public int? ExperimentId { get; set; }
    }
}
