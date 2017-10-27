using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BcsAdmin.DAL.Models
{
    public partial class EpEntity
    {
        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string VisualisationXml { get; set; }
        public string Type { get; set; }
        public int? Active { get; set; }

        [ForeignKey(nameof(ParentId))]
        public virtual ICollection<EpEntity> Children { get; set; }

        //[ForeignKey(nameof(EpEntityLocation))]
        //public virtual ICollection<EpEntityLocation> Locations { get; set; }

    }
}
