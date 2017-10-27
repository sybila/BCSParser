using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpModelReactionToReaction
    {
        public int Id { get; set; }
        public int? ModelReactionId { get; set; }
        public int? ReactionId { get; set; }
    }
}
