using System.Collections.Generic;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    public class BcsReactantNode : BcsExpressionNode
    {
        public double Coeficient { get; set; } = 1.0;
        public BcsExpressionNode Complex { get; set; }

        public TextRange CoeficientRange { get; set; }

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return new [] { Complex };
        }
    }
}
