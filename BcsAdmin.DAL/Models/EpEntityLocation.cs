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

        [ForeignKey(nameof(EpEntity))]
        public int? ParentEntityId { get; set; }

        [ForeignKey(nameof(EpEntity))]
        public int? ChildEntityId { get; set; }

        public EpEntity Parent { get; set; }
        public EpEntity Child { get; set; }
    }
}
