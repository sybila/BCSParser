using System.Diagnostics;
using System.Linq;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[C: {ToDisplayString()}]")]
    public sealed class BcsComplexNode : BcsComposedEntity
    {
        public override string ToDisplayString() =>
            $"{string.Join(".", Parts.Select(p => p.ToDisplayString()))}";
    }
}