using System.Collections.Generic;
using System.Linq;

namespace BcsResolver.SemanticModel
{
    public sealed class BcsComplexSymbol : BcsComposedSymbol
    {
        public IEnumerable<BcsComponentSymbol> Components => Parts.OfType<BcsComponentSymbol>();

        public BcsComplexSymbol()
        {
            Type = BcsSymbolType.Complex;
        }
    }
}
