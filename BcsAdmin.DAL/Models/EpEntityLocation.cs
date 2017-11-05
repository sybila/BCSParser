using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BcsAdmin.DAL.Models
{
    public partial class EpEntityLocation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Location))]
        public int? ParentEntityId { get; set; }

        [ForeignKey(nameof(Entity))]
        public int? ChildEntityId { get; set; }

        public EpEntity Location { get; set; }
        public EpEntity Entity { get; set; }
    }
}
