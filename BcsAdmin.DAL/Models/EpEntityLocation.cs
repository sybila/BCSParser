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

        public int? ParentEntityId { get; set; }
        public int? ChildEntityId { get; set; }

        public EpEntity ParentEntity { get; set; }
        public EpEntity ChildEntity { get; set; }
    }
}
