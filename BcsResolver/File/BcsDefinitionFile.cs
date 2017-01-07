using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.File
{
    public class BcsDefinitionFile
    {
        public List<BcsEntity> Entities { get; private set; } = new List<BcsEntity>();
        public List<BcsRule> Rules { get; private set; } = new List<BcsRule>();
        public List<string> Artifacts { get; private set; } = new List<string>();
    }
}
