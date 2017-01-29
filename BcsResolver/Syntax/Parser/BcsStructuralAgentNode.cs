using System.Diagnostics;
using System.Linq;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[SA: {ToDisplayString()}]")]
    public sealed class BcsStructuralAgentNode : BcsComposedEntityNode
    {
        public override string ToDisplayString() =>
            $"{Identifier?.ToDisplayString()??""}{Parts.ToDisplayString()}";
    }
}