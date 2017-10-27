using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpUnit
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string ParentTree { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double? Rate { get; set; }
    }
}
