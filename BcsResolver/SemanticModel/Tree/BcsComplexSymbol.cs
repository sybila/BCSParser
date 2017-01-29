using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BcsResolver.SemanticModel.Tree
{
    [DebuggerDisplay("[{ToDisplayString()}]")]
    public sealed class BcsComplexSymbol : BcsComposedSymbol
    {
        public IEnumerable<BcsStructuralAgentSymbol> StructuralAgents => Parts.OfType<BcsStructuralAgentSymbol>();

        public BcsComplexSymbol()
        {
            Type = BcsSymbolType.Complex;
        }
    }
}
