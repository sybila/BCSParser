using BcsResolver.SemanticModel.Tree;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using BcsAdmin.DAL.Models;

namespace BcsAdmin.BL.Services
{
    public class SemanticModelFactory : SymbolFactory
    {
        public ConcurrentDictionary<string, BcsNamedSymbol> namedSymbols { get; } = new ConcurrentDictionary<string, BcsNamedSymbol>();
        public ISet<string> seenNames { get; } = new HashSet<string>();

        public override BcsNamedSymbol CreateSymbol(EpEntity entity)
        {
            var name = entity.Code;

            if(!namedSymbols.ContainsKey(name))
            {
                if(!seenNames.Add(name))
                {
                    return CreateError(entity, new Exception("There is a dependency cycle"));
                }
                namedSymbols[name] = base.CreateSymbol(entity);
            }
            return namedSymbols[name];
        }
    }
}
