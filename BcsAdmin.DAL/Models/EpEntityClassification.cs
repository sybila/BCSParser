using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BcsAdmin.DAL.Models
{
    public partial class EpEntityClassification : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public int? EntityId { get; set; }
        public int? ClassificationId { get; set; }

        public EpEntity Entity { get; set; }
        public EpClassification Classification { get; set; }
    }
}
