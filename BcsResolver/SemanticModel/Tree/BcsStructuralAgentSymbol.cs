using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BcsResolver.SemanticModel.Tree
{
    [DebuggerDisplay("[{ToDisplayString()}]")]
    public sealed class BcsStructuralAgentSymbol : BcsComposedSymbol
    {
        public IEnumerable<BcsAtomicAgentSymbol> AtomicAgents => Parts.OfType<BcsAtomicAgentSymbol>();

        public BcsStructuralAgentSymbol()
        {
            Type = BcsSymbolType.StructuralAgent;
        }
    }
}
