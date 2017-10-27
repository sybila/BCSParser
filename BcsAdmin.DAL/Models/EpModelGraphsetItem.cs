using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpModelGraphsetItem
    {
        public int Id { get; set; }
        public int? GraphsetId { get; set; }
        public int? ModelSpecieId { get; set; }
        public string Side { get; set; }
    }
}
