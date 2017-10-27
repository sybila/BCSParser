using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpNumbersValues
    {
        public int Id { get; set; }
        public int? AttributeId { get; set; }
        public int UnitId { get; set; }
        public double ValueFrom { get; set; }
        public double ValueTo { get; set; }
        public int? ErrorMargin { get; set; }
        public int ExperimentId { get; set; }
        public int OrganismId { get; set; }
        public int Author { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }

        public EpNumbersAttributes Attribute { get; set; }
    }
}
