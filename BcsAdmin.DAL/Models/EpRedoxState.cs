using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpRedoxState
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mark { get; set; }
        public int? Active { get; set; }
    }
}
