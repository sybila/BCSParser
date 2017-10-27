using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpModelDataset
    {
        public int Id { get; set; }
        public int? ModelId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public string Solver { get; set; }
        public int? Public { get; set; }
    }
}
