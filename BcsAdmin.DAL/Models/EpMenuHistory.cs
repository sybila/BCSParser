using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpMenuHistory
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string NameUrl { get; set; }
        public DateTime? Date { get; set; }
    }
}
