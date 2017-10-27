using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpEntityOrganism
    {
        public int Id { get; set; }
        public int? OrganismId { get; set; }
        public int? EntityId { get; set; }
        public string GeneGroup { get; set; }
    }
}
