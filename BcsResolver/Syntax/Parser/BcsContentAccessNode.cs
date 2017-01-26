using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[Acc: {ToDisplayString()}]")]
    public class BcsContentAccessNode : BcsExpressionNode
    {
        public BcsExpressionNode Container { get; set; }
        public BcsNamedEntityNode Child { get; set; }
        public TextRange Operator { get; set; }

        public override string ToDisplayString() =>
            $"{Child.ToDisplayString()}::{Container.ToDisplayString()}";

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return base.EnumerateChildNodes().Concat(new[] { Child, Container});
        }
    }
}