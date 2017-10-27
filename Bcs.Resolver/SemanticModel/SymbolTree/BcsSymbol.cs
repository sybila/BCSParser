using System.Collections.Generic;
using BcsResolver.Common;

namespace BcsResolver.SemanticModel.Tree
{
    public abstract class BcsSymbol : IBcsTreeNode<BcsSymbol>
    {
        public BcsSymbolType Type { get; protected set; } = BcsSymbolType.Unknown;
        public BcsSymbol Parent { get; set; }

        public virtual IEnumerable<BcsSymbol> EnumerateChildNodes()
        {
            return new List<BcsSymbol>();
        }

        public virtual string ToDisplayString() => Type.ToString();
    }
}
