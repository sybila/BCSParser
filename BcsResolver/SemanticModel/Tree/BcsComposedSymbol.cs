using System.Collections.Generic;

namespace BcsResolver.SemanticModel
{
    public abstract class BcsComposedSymbol : BcsNamedSymbol
    {
        public BcsSymbolType BcsSymbolType { get; set; }
        public List<BcsNamedSymbol> Parts { get; set; }
        public List<BcsLocationSymbol> Locations { get; set; }
    }
}
