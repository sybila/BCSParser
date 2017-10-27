using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpReactionItemSpecinstance
    {
        public int Id { get; set; }
        public int? ChildEntityId { get; set; }
        public int? ParentItemId { get; set; }
        public string Type { get; set; }
    }
}
