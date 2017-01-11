using System.Diagnostics;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[Acc: {ToDisplayString()}]")]
    public class BcsContentAccessNode : BcsExpressionNode
    {
        public BcsExpressionNode Target { get; set; }
        public BcsNamedEntityNode Container { get; set; }
        public TextRange Operator { get; set; }

        public override string ToDisplayString() =>
            $"{Target.ToDisplayString()}::{Container.ToDisplayString()}";
    }
}