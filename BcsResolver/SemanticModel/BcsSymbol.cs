using System;
using System.Collections.Generic;
using BcsResolver.Common;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.SemanticModel
{
    public abstract class BcsSymbol : IBcsTreeNode<BcsSymbol>
    {
        public BcsSymbolType Type { get; protected set; } = BcsSymbolType.Unknown;
        public BcsSymbol Parent { get; set; }

        public virtual IEnumerable<BcsSymbol> EnumerateChildNodes()
        {
            return new List<BcsSymbol>();
        }
    }
}
