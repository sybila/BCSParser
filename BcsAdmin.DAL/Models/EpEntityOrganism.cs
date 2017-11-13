using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpEntityOrganism : IEntity<int>
    {
        public int Id { get; set; }
        public int? OrganismId { get; set; }
        public int? EntityId { get; set; }
        public string GeneGroup { get; set; }
    }
}
