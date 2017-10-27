using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpAnnotation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ItemId { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }
        public string TermId { get; set; }
        public string TermType { get; set; }
        public int? Default { get; set; }
        public int? Active { get; set; }
    }
}
