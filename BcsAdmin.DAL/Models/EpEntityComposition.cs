using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BcsAdmin.DAL.Models
{
    public partial class EpEntityComposition
    {
        [Key]
        public int Id { get; set; }
        public int? ParentEntityId { get; set; }
        public int? ChildEntityId { get; set; }
        public string Type { get; set; }

        public EpEntity ParentEntity { get; set; }
        public EpEntity ChildEntity { get; set; }

        public override string ToString() => $"{ParentEntity.ToString()} <- {ChildEntity.ToString()}";
    }
}
