using BcsResolver.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.Parser
{
    public class BcsReactantNode : BcsExpressionNode
    {
        public double Coeficient { get; set; } = 1.0;
        public BcsComplexNode Complex { get; set; }

        public TextRange CoeficientRange { get; set; }

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return new BcsExpressionNode[] { Complex };
        }
    }
}
