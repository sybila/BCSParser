using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpEntityNote : IEntity<int>
    {
        public int Id { get; set; }
        public int? EntityId { get; set; }
        public int? UserId { get; set; }
        public string Text { get; set; }
        public DateTime? Inserted { get; set; }
        public DateTime? Updated { get; set; }

        public EpEntity Entity { get; set; }
        public EpUser User { get; set; }
    }
}
