using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpCountry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public int? LanguageId { get; set; }
        public string Continent { get; set; }
        public string Code { get; set; }
        public int? Order { get; set; }
        public int? Active { get; set; }
    }
}
