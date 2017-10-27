using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpEntityComposition
    {
        public int Id { get; set; }
        public int? ParentEntityId { get; set; }
        public int? ChildEntityId { get; set; }
        public string Type { get; set; }
    }
}
