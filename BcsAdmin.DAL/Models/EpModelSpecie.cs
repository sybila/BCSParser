using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpModelSpecie
    {
        public int Id { get; set; }
        public int? ModelId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string EquationType { get; set; }
        public int? UnitId { get; set; }
        public string InitExpression { get; set; }
        public string DynExpression { get; set; }
    }
}
