using BcsResolver.Extensions;
using BcsResolver.SemanticModel.Tree;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bcs.Resolver.Common
{
    public static class SymbolHelper
    {
        public static IReadOnlyDictionary<string, IReadOnlyList<BcsComposedSymbol>> ToLocationSymbolMap(this IEnumerable<BcsComposedSymbol> allEntities)
        {
            var symbolByLocation = new ConcurrentDictionary<string, List<BcsComposedSymbol>>();

            foreach (var entity in allEntities)
            {
                foreach (var location in entity.Locations)
                {
                    symbolByLocation.GetOrAdd(location?.Name ?? "no-name", new List<BcsComposedSymbol>()).Add(entity);
                }
            }
            return symbolByLocation.ToDictionary(k => k.Key, v => v.Value.As<IReadOnlyList<BcsComposedSymbol>>());
        }
    }
}
