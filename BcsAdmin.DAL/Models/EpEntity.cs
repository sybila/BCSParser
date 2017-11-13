using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BcsAdmin.DAL.Models
{
    public enum HierarchyType
    {
        State = 0,
        Compartment = 1,
        Complex = 2,
        Structure = 3,
        Atomic = 4,
    }

    public class EpEntity : IEntity<int>
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
        public HierarchyType HierarchyType { get; set; }

        public EpEntity Parent { get; set; }

        public virtual ICollection<EpEntity> Children { get; set; } = new List<EpEntity>();
        public virtual ICollection<EpEntityLocation> Locations { get; set; } = new List<EpEntityLocation>();
        public virtual ICollection<EpEntityComposition> Components { get; set; } = new List<EpEntityComposition>();
        public virtual ICollection<EpEntityClassification> Classifications { get; set; } = new List<EpEntityClassification>();
        public virtual ICollection<EpEntityNote> Notes { get; set; } = new List<EpEntityNote>();

        public override string ToString() => $"{HierarchyType.ToString()}: ({Code}) {Name}";
    }
}
