using System.Diagnostics;

namespace BcsResolver.SemanticModel.Tree
{
    [DebuggerDisplay("[{ToDisplayString()}]")]
    public sealed class BcsStateSymbol : BcsNamedSymbol
    {
        public BcsStateSymbol()
        {
            Type = BcsSymbolType.State;
        }
    }
}
