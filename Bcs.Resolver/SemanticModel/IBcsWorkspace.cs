using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BcsResolver.SemanticModel.Tree;

namespace BcsResolver.SemanticModel
{
    public interface IBcsWorkspace
    {
        IReadOnlyDictionary<string, BcsComplexSymbol> Complexes { get; }
        IReadOnlyDictionary<string, BcsStructuralAgentSymbol> StructuralAgents { get; }
        IReadOnlyDictionary<string, BcsAtomicAgentSymbol> AtomicAgents { get; }
        IReadOnlyDictionary<string, BcsCompartmentSymbol> Locations { get; }
        IReadOnlyDictionary<string, IReadOnlyList<BcsComposedSymbol>> LocationEntityMap { get; }
        IEnumerable<BcsComposedSymbol> GetAllEntities();
        Task CreateSemanticModelAsync(CancellationToken cencellationToken);

        IReadOnlyList<SemanticError> Errors { get; }
    }
}
