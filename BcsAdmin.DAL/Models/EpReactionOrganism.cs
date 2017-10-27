using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpReactionOrganism
    {
        public int Id { get; set; }
        public int? OrganismId { get; set; }
        public int? ReactionId { get; set; }
    }
}
