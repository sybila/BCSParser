using System.Collections.Generic;
using System.Diagnostics;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[Id: {ToDisplayString()}]")]
    public class BcsIdentifierNode : BcsExpressionNode
    {
        public string Name { get; set; }
        public TextRange NameRange { get; set; }
        public override string ToDisplayString() => Name;
    }
}
