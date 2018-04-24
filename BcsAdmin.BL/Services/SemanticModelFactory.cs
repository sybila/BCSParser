using BcsResolver.SemanticModel.Tree;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using BcsAdmin.DAL.Models;
using System.Threading.Tasks;
using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Services
{
    public class ApiSemanticModelFactory : SymbolFactory
    {
        public ApiSemanticModelFactory(Func<int, ApiEntity> entityProvider) : base(entityProvider)
        {
        }

        public ConcurrentDictionary<int, BcsNamedSymbol> namedSymbols { get; } = new ConcurrentDictionary<int, BcsNamedSymbol>();
        public ISet<int> seenNames { get; } = new HashSet<int>();

        public override BcsNamedSymbol CreateSymbol(int id)
        {
            if(!namedSymbols.ContainsKey(id))
            {
                if(!seenNames.Add(id))
                {
                    return CreateError("x", new Exception("There is a dependency cycle"));
                }
                namedSymbols[id] = base.CreateSymbol(id);
            }
            return namedSymbols[id];
        }
    }
}
