using System.Collections.Generic;
using System.Linq;

namespace BcsResolver.SemanticModel
{
    public sealed class BcsComponentSymbol : BcsComposedSymbol
    {
        public IEnumerable<BcsAgentSymbol> Agents => Parts.OfType<BcsAgentSymbol>();

        public BcsComponentSymbol()
        {
            Type = BcsSymbolType.Component;
        }
    }
}
