using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpReactionEquationEntity
    {
        public int Id { get; set; }
        public int ReactionId { get; set; }
        public int? EntityId { get; set; }
        public string Type { get; set; }
        public string Specification { get; set; }
        public sbyte Count { get; set; }
        public sbyte StartPos { get; set; }
        public sbyte EndPos { get; set; }
    }
}
