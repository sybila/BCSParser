using System.Diagnostics;
using System.Linq;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[SA: {ToDisplayString()}]")]
    public sealed class BcsStructuralAgentNode : BcsComposedEntity
    {
        public TextRange BeginBrace { get; set; }
        public TextRange EndBrace { get; set; }
        public override string ToDisplayString() =>
            $"{Identifier.ToDisplayString()}({string.Join(",", Parts.Select(p => p.ToDisplayString()))})";
    }
}