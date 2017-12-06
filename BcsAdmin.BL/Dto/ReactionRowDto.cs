using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Dto
{
    public class ReactionRowDto : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Equation { get; set; }
        public string Modifier { get; set; }
    }
}
