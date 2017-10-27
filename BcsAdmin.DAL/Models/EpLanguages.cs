using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpLanguages
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
        public int? ActiveWeb { get; set; }
        public int? ActiveAdmin { get; set; }
        public int? Order { get; set; }
    }
}
