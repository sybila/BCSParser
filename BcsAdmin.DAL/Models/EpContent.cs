using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpContent
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
    }
}
