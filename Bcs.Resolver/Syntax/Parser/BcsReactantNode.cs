using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[Rct: {ToDisplayString()}]")]
    public class BcsReactantNode : BcsExpressionNode
    {
        public double Coeficient { get; set; } = 1.0;
        public BcsExpressionNode Complex { get; set; }

        public TextRange CoeficientRange { get; set; }

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return new [] { Complex };
        }

        public override string ToDisplayString()
        {
            return $"{Coeficient.ToString(CultureInfo.InvariantCulture)}{Complex.ToDisplayString()}";
        }
    }
}
