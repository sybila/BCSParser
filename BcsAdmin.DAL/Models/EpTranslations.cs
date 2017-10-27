using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpTranslations
    {
        public int Id { get; set; }
        public int LangId { get; set; }
        public string Name { get; set; }
        public string Trans { get; set; }
    }
}
