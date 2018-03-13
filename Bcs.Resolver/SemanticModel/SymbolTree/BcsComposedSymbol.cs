using System.Collections.Generic;
using System.Linq;

namespace BcsResolver.SemanticModel.Tree
{
    public abstract class BcsComposedSymbol : BcsNamedSymbol
    {
        public BcsSymbolType BcsSymbolType { get; set; }
        public List<BcsNamedSymbol> Parts { get; set; }
        public List<BcsNamedSymbol> Locations { get; set; }

        public override string ToDisplayString() => $"{Type}: {Name}::{string.Join(",", Locations?.Select(l => l.Name)?? new [] {"none"})}";

        public override IEnumerable<BcsSymbol> EnumerateChildNodes()
        {
            return base.EnumerateChildNodes()
                .Concat(Parts)
                .Concat(Locations);
        }
    }
}
