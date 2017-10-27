using System.Diagnostics;
using System.Linq;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[C: {ToDisplayString()}]")]
    public sealed class BcsComplexNode : BcsComposedEntityNode
    {
        public override string ToDisplayString() => Parts.ToDisplayString();
    }
}