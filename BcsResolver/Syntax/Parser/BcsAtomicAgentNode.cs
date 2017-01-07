using System.Diagnostics;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[A: {Name}...]")]
    public sealed class BcsAtomicAgentNode : BcsComposedEntity
    {
        public TextRange BeginBrace { get; set; }
        public TextRange EndBrace { get; set; }
    }
}