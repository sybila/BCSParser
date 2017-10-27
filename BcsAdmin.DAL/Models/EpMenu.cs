using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpMenu
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int LangId { get; set; }
        public string Name { get; set; }
        public string NameUrl { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKeys { get; set; }
        public string MetaTitle { get; set; }
        public string H1Title { get; set; }
        public string Type { get; set; }
        public int? Order { get; set; }
        public int? Default { get; set; }
        public int? Auth { get; set; }
        public int? Active { get; set; }
    }
}
