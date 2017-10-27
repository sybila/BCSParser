using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpFunctionItem
    {
        public int Id { get; set; }
        public int? FunctionId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? UnitId { get; set; }
        public int? Multiple { get; set; }
    }
}
