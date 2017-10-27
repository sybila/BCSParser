using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpAttributeValue
    {
        public int Id { get; set; }
        public int? AttributeId { get; set; }
        public int? ItemId { get; set; }
        public string Short { get; set; }
        public DateTime? Datetime { get; set; }
        public double? Value { get; set; }
        public string Text { get; set; }
    }
}
