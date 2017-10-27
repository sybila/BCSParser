using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpModelReactionItem
    {
        public int Id { get; set; }
        public int? ModelReactionId { get; set; }
        public int? FunctionItemId { get; set; }
        public int? ModelSpecieId { get; set; }
        public int? IsGlobal { get; set; }
        public double? Value { get; set; }
        public int? Stoichiometry { get; set; }
    }
}
