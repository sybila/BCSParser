using System.Diagnostics;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("Components = [{string.Join(\", \",Components.Select(c=> c?.Name ?? \"()\"))}]")]
    public sealed class BcsComplexNode : BcsComposedEntity
    {
    }
}