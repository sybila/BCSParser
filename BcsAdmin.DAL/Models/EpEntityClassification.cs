using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpEntityClassification
    {
        public int Id { get; set; }
        public int? EntityId { get; set; }
        public int? ClassificationId { get; set; }
    }
}
