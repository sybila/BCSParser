using System.Collections.Generic;
using System.Diagnostics;

namespace BcsResolver.SemanticModel.Tree
{
    [DebuggerDisplay("[{ToDisplayString()}]")]
    public class BcsRuleSymbol : BcsNamedSymbol
    {
        public BcsNamedSymbol Modifier { get; set; }
        public List<BcsNamedSymbol> LeftSideReactants { get; set; }
        public List<BcsNamedSymbol> RightSideReactants { get; set; }

        public BcsRuleSymbol()
        {
            Type = BcsSymbolType.Rule;
        }
    }
}
