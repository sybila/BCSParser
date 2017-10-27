using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Extensions;
using BcsResolver.File;
using BcsResolver.SemanticModel.Tree;

namespace BcsResolver.SemanticModel.SymbolTree
{
    [DebuggerDisplay("[{ToDisplayString()}]")]
    public sealed class BcsVariableSymbol : BcsNamedSymbol
    {
        public IReadOnlyList<BcsNamedSymbol> AssignedEntities { get; set; } = new List<BcsNamedSymbol>();
        public BcsSymbolType VariableType { get; set; }

        public BcsVariableSymbol()
        {
            Type=BcsSymbolType.Variable;
        }

        public override IEnumerable<BcsSymbol> EnumerateChildNodes()
            => AssignedEntities;

        public override string ToDisplayString()
            => $"{base.ToDisplayString()} type: {VariableType.GetDescription()} {{{string.Join(", ", AssignedEntities.Select(e=> e.ToDisplayString()))}}}";
    }
}
