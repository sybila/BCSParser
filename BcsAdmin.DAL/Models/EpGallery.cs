using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpGallery
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Filename { get; set; }
        public string Desc { get; set; }
        public int? Order { get; set; }
    }
}
