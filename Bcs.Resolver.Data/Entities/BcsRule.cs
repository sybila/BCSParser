using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.File
{
    public class BcsRule : BcsFileRecord
    {
        public string Equation { get; set; }
        public string Modifier { get; set; }
    }
}
