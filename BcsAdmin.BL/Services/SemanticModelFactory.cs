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

        public override BcsNamedSymbol CreateSymbol(EpEntity entity)
        {
            var name = entity.Code;

            if(!namedSymbols.ContainsKey(name))
            {
                namedSymbols[name] = base.CreateSymbol(entity);
            }
            return namedSymbols[name];
        }
    }
}
