using BcsResolver.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.SemanticModel;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Tokenizer;
using BcsResolver.File;
using Bcs.Resolver.Common;

namespace BcsResolver.File
{

    public class BcsFileWorkspace : IBcsWorkspace
    {
        private readonly IBcsEntityMetadataProvider entityMetadataProvider;
        private BcsFileEntityBinder entityBinder;

        public BcsFileWorkspace(IBcsEntityMetadataProvider entityMetadataProvider)
        {
            this.entityMetadataProvider = entityMetadataProvider;
        }

        public IReadOnlyDictionary<string, BcsComplexSymbol> Complexes { get; private set; }
        public IReadOnlyDictionary<string, BcsStructuralAgentSymbol> StructuralAgents { get; private set; }
        public IReadOnlyDictionary<string, BcsAtomicAgentSymbol> AtomicAgents { get; private set; }
        public IReadOnlyDictionary<string, BcsLocationSymbol> Locations { get; set; }
        public IReadOnlyDictionary<string, IReadOnlyList<BcsComposedSymbol>> LocationEntityMap { get; set; }

        public IEnumerable<BcsComposedSymbol> GetAllEntities()
            => AtomicAgents.Values
                .Concat(StructuralAgents.Values.Cast<BcsComposedSymbol>())
                .Concat(Complexes.Values);


        public void CreateSemanticModel()
        {
            entityBinder = new BcsFileEntityBinder(entityMetadataProvider);

            var resolvedSymbols = entityBinder.BindEntities().ToLookup(k => k.Type);

            Complexes = resolvedSymbols[BcsSymbolType.Complex].Cast<BcsComplexSymbol>().ToDictionary(k => k.Name);
            StructuralAgents =
                resolvedSymbols[BcsSymbolType.StructuralAgent].Cast<BcsStructuralAgentSymbol>()
                    .ToDictionary(k => k.Name);
            AtomicAgents = resolvedSymbols[BcsSymbolType.Agent].Cast<BcsAtomicAgentSymbol>().ToDictionary(k => k.Name);

            var allEntities = Complexes.Values
                .Concat<BcsComposedSymbol>(StructuralAgents.Values)
                .Concat<BcsComposedSymbol>(AtomicAgents.Values).ToList();

            Locations = allEntities
                .SelectMany(ce => ce.Locations)
                .Distinct()
                .ToDictionary(k => k.Name);

            LocationEntityMap = allEntities.ToLocationSymbolMap();
        }
    }
}
