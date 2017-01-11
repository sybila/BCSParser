using System.Diagnostics;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[S: {Container}]")]
    public class BcsContentAccessNode : BcsExpressionNode
    {
        public BcsExpressionNode Target { get; set; }
        public BcsNamedEntityNode Container { get; set; }
        public TextRange Operator { get; set; }
    }
}