using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpSetup
    {
        public int Id { get; set; }
        public int? LangId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string Default { get; set; }
        public string Description { get; set; }
    }
}
