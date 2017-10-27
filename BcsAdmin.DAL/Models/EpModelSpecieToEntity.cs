using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpModelSpecieToEntity
    {
        public int Id { get; set; }
        public int? ModelSpecieId { get; set; }
        public int? EntityId { get; set; }
        public int? LocationId { get; set; }
    }
}
