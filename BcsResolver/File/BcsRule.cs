using BcsResolver.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.File
{
    public class BcsRule : BcsFileRecord
    {
        public BcsExpressionAdapter<BcsReactionNode> Equation { get; private set; } = new BcsExpressionAdapter<BcsReactionNode>();
        public BcsExpressionAdapter<BcsComplexNode> Modifier { get; set; } = new BcsExpressionAdapter<BcsComplexNode>();
    }
}
