using System.Collections.Generic;
using BcsResolver.SemanticModel.Tree;

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
}
