using System.Diagnostics;

namespace BcsResolver.SemanticModel.Tree
{
    [DebuggerDisplay("[{ToDisplayString()}]")]
    public sealed class BcsLocationSymbol : BcsNamedSymbol
    {
        public BcsLocationSymbol()
        {
            Type = BcsSymbolType.Location;
        }

        public override string ToDisplayString() => Name;
    }
}
