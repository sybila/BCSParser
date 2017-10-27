using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpReactionItemComposition
    {
        public int Id { get; set; }
        public int? ParentItemId { get; set; }
        public int? ChildEntityId { get; set; }
        public string Type { get; set; }
    }
}
