using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.BL.Dto
{
    public class SuggestionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString() => $"{Name} ({Description})";
    }
}
