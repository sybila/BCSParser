using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.SemanticModel;

namespace BcsResolver.File
{
    public class BcsEntity : BcsFileRecord
    {
        public List<string> States { get; } = new List<string>();
        public List<string> Locations { get; } = new List<string>();
        public List<string> Composition { get; } = new List<string>();
        public BcsEntityType Type { get; set; }
    }
}
