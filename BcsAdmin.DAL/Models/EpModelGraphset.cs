using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpModelGraphset
    {
        public int Id { get; set; }
        public int? ModelId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Active { get; set; }
    }
}
