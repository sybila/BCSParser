using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Datatype { get; set; }
        public string Type { get; set; }
        public string Default { get; set; }
        public string Style { get; set; }
        public int? Order { get; set; }
        public int? Required { get; set; }
        public int? Active { get; set; }
    }
}
