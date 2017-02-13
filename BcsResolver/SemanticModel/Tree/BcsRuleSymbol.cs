using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BcsResolver.SemanticModel.Tree
{
    [DebuggerDisplay("[{ToDisplayString()}]")]
    public class BcsRuleSymbol : BcsNamedSymbol
    {
        public BcsNamedSymbol Modifier { get; set; }
        public List<BcsNamedSymbol> LeftSideReactants { get; set; }
        public List<BcsNamedSymbol> RightSideReactants { get; set; }

        public override IEnumerable<BcsSymbol> EnumerateChildNodes()
        {
            return base.EnumerateChildNodes()
                .Concat(LeftSideReactants)
                .Concat(RightSideReactants);
        }

        public BcsRuleSymbol()
        {
            Type = BcsSymbolType.Rule;
        }
    }
}
