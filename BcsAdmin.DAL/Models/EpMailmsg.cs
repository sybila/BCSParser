using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpMailmsg
    {
        public int Id { get; set; }
        public int? LangId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
    }
}
