using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpReactionClassification
    {
        public int Id { get; set; }
        public int? ReactionId { get; set; }
        public int? ClassificationId { get; set; }
    }
}
