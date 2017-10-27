using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpReactionEquationVariable
    {
        public int Id { get; set; }
        public int ReactionId { get; set; }
        public string Variable { get; set; }
        public int EntityId { get; set; }
        public sbyte StartPos { get; set; }
        public sbyte EndPos { get; set; }
    }
}
