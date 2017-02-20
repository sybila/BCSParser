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

namespace BcsResolver.File
{
    public interface IBcsWorkspace
    {
        IReadOnlyDictionary<string, BcsComplexSymbol> Complexes { get; }
        IReadOnlyDictionary<string, BcsStructuralAgentSymbol> StructuralAgents { get; }
        IReadOnlyDictionary<string, BcsAtomicAgentSymbol> AtomicAgents { get; }
        IReadOnlyDictionary<string, BcsLocationSymbol> Locations { get; }
        IReadOnlyDictionary<string, IReadOnlyList<BcsComposedSymbol>> LocationEntityMap { get; }

        void CreateSemanticModel();
    }

    public class BcsWorkspace : IBcsWorkspace
    {
        private readonly IBcsEntityMetadataProvider entityMetadataProvider;
        private BcsEntityBinder entityBinder;

        public BcsWorkspace(IBcsEntityMetadataProvider entityMetadataProvider)
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
            entityBinder = new BcsEntityBinder(entityMetadataProvider);

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

            LocationEntityMap = CreateLocationSymbolMap(allEntities);
        }

        private static IReadOnlyDictionary<string, IReadOnlyList<BcsComposedSymbol>> CreateLocationSymbolMap(List<BcsComposedSymbol> allEntities)
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
