using System.Diagnostics;

namespace BcsResolver.SemanticModel.Tree
{
    [DebuggerDisplay("[{ToDisplayString()}]")]
    public sealed class BcsCompartmentSymbol : BcsNamedSymbol
    {
        public BcsCompartmentSymbol()
        {
            Type = BcsSymbolType.Location;
        }

        public override string ToDisplayString() => Name;
    }
}
