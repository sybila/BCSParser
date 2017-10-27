using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpModelParameter
    {
        public int Id { get; set; }
        public int? ModelId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Solver { get; set; }
    }
}
