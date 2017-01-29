using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BcsResolver.SemanticModel.Tree
{
    [DebuggerDisplay("[{ToDisplayString()}]")]
    public sealed class BcsAtomicAgentSymbol : BcsComposedSymbol
    {
        public IEnumerable<BcsStateSymbol> States => Parts.OfType<BcsStateSymbol>();

        public BcsAtomicAgentSymbol()
        {
            Type = BcsSymbolType.Agent;
        }
    }
}
