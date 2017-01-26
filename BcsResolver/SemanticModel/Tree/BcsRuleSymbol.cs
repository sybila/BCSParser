using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.SemanticModel
{
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
