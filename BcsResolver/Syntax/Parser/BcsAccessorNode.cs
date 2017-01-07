using System.Diagnostics;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[S: {Name}]")]
    public class BcsAccessorNode : BcsExpressionNode
    {
        public BcsExpressionNode Target { get; set; }
        public BcsIdentifierNode Name { get; set; }

        public TextRange Operator { get; set; }
    }
}