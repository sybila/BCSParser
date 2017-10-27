using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpModelReactionItemToReactionItem
    {
        public int Id { get; set; }
        public int? ModelReactionItemId { get; set; }
        public int? ReactionItemId { get; set; }
    }
}
