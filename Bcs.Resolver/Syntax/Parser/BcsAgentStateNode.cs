using System.Diagnostics;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[AS: {ToDisplayString()}]")]
    public sealed class BcsAgentStateNode : BcsNamedEntityNode
    {
        public override string ToDisplayString() => Identifier?.ToDisplayString() ?? "";

    }
}