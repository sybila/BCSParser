using System.Collections.Generic;
using System.Linq;

namespace BcsResolver.SemanticModel
{
    public sealed class BcsAgentSymbol : BcsComposedSymbol
    {
        public IEnumerable<BcsStateSymbol> States => Parts.OfType<BcsStateSymbol>();

        public BcsAgentSymbol()
        {
            Type = BcsSymbolType.Agent;
        }
    }
}
