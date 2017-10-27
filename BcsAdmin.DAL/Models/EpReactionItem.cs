using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpReactionItem
    {
        public int Id { get; set; }
        public int? ReactionId { get; set; }
        public int? EntityId { get; set; }
        public int? IsComposition { get; set; }
        public int? SpecEntityId { get; set; }
        public int? LocationId { get; set; }
        public string Type { get; set; }
        public int? Stoichiometry { get; set; }
        public string VarValue { get; set; }
    }
}
