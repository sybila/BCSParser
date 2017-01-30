using BcsResolver.Extensions;
using System;
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

        public IReadOnlyDictionary<string, BcsLocationSymbol> Locations { get; private set; }

        public void CreateSemanticModel()
        {
            entityBinder = new BcsEntityBinder(entityMetadataProvider);

            var resolvedSymbols =entityBinder.BindEntities().ToLookup(k => k.Type);

            Complexes = resolvedSymbols[BcsSymbolType.Complex].Cast<BcsComplexSymbol>().ToDictionary(k=> k.Name);
            StructuralAgents = resolvedSymbols[BcsSymbolType.StructuralAgent].Cast<BcsStructuralAgentSymbol>().ToDictionary(k => k.Name);
            AtomicAgents = resolvedSymbols[BcsSymbolType.Agent].Cast<BcsAtomicAgentSymbol>().ToDictionary(k => k.Name);
            Locations = 
                Complexes.Values
                .Concat<BcsComposedSymbol>(StructuralAgents.Values)
                .Concat<BcsComposedSymbol>(AtomicAgents.Values)
                .SelectMany(ce=> ce.Locations)
                .Distinct()
                .ToDictionary(k=> k.Name);
        }


    }
}
