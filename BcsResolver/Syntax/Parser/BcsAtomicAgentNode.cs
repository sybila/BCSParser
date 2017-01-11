using System.Diagnostics;
using System.Linq;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[AA: {ToDisplayString()}]")]
    public sealed class BcsAtomicAgentNode : BcsComposedEntity
    {
        public TextRange BeginBrace { get; set; }
        public TextRange EndBrace { get; set; }

        public override string ToDisplayString() => 
            $"{Identifier.ToDisplayString()}{{{string.Join(",",Parts.Select(p=>p.ToDisplayString()))}}}";
    }
}