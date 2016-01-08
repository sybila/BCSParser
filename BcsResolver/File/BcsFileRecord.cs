using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.File
{
    public abstract class BcsFileRecord
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Organism { get; set; } = string.Empty;
        public string Classification { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Links { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public List<string> MalformedLines { get; private set; } = new List<string>();
    }
}
