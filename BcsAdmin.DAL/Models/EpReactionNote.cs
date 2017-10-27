using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpReactionNote
    {
        public int Id { get; set; }
        public int? ReactionId { get; set; }
        public int? UserId { get; set; }
        public string Text { get; set; }
        public DateTime? Inserted { get; set; }
        public DateTime? Updated { get; set; }
    }
}
