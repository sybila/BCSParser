using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BcsAdmin.DAL.Models
{
    public partial class EpEntityComposition : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ComposedEntity))]
        public int? ParentEntityId { get; set; }

        [ForeignKey(nameof(Component))]
        public int? ChildEntityId { get; set; }
        public string Type { get; set; }

        public EpEntity ComposedEntity { get; set; }
        public EpEntity Component { get; set; }

        public override string ToString() => $"{ComposedEntity.ToString()} <- {Component.ToString()}";
    }
}
