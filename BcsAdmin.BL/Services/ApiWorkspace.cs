using Bcs.Resolver.Common;
using BcsAdmin.DAL.Api;
using BcsResolver.Extensions;
using BcsResolver.SemanticModel;
using BcsResolver.SemanticModel.Tree;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Services
{
    public class ApiWorkspace : IBcsWorkspace
    {
        private bool isInitialized;

        private readonly IRepository<ApiEntity, int> entityRepositry;

        public IReadOnlyDictionary<string, BcsComplexSymbol> Complexes { get; private set; }

        public IReadOnlyDictionary<string, BcsStructuralAgentSymbol> StructuralAgents { get; private set; }

        public IReadOnlyDictionary<string, BcsAtomicAgentSymbol> AtomicAgents { get; private set; }

        public IReadOnlyDictionary<string, BcsCompartmentSymbol> Locations { get; private set; }

        public IReadOnlyDictionary<string, IReadOnlyList<BcsComposedSymbol>> LocationEntityMap { get; private set; }

        public IReadOnlyList<SemanticError> Errors { get; private set; }

        public ApiWorkspace(IRepository<ApiEntity, int> entityRepositry)
        {
            this.entityRepositry = entityRepositry;
        }

        public IEnumerable<BcsComposedSymbol> GetAllEntities()
           => AtomicAgents.Values
               .Concat(StructuralAgents.Values.Cast<BcsComposedSymbol>())
               .Concat(Complexes.Values);

        public async Task CreateSemanticModelAsync(CancellationToken token)
        {
            if (isInitialized)
            {
                return;
            }

            var errors = new List<SemanticError>();
            var stubs = await ApiHelper.GetWebDataAsync<ApiQueryEntity>(token, "https://api.e-cyanobacterium.org", "entities");
            var ids = stubs.Select(e => e.Id).ToArray();
            var results = await GetEntitiesAsync(ids);



            var entities = results.ToDictionary(e => e.Id);

            var semanticModelFactory = new ApiSemanticModelFactory(id => entities.GetValueOrDefault(id));
            var symbols = ids.Select(id => semanticModelFactory.CreateSymbol(id)).Distinct().ToList();

            var symbolNameGrouping = symbols.GroupBy(s => s.Name);

            var duplicities = symbolNameGrouping
                .Where(g => g.Count() > 1)
                .Select(dg => string.Join(", ", dg.Select(d => $"{d.ToDisplayString()}")));

            errors.AddRange(duplicities.Select(d => new SemanticError($"There are entities with the same name: {d}", SemanticErrorSeverity.Warning)));

            var nonduplicitSymbols = symbolNameGrouping.Select(sg => sg.First());


            AtomicAgents = nonduplicitSymbols
                .OfType<BcsAtomicAgentSymbol>()
                .ToDictionary(k => k.Name);

            StructuralAgents = nonduplicitSymbols
                .OfType<BcsStructuralAgentSymbol>()
                .ToDictionary(k => k.Name);

            Complexes = nonduplicitSymbols
                .OfType<BcsComplexSymbol>()
                .ToDictionary(k => k.Name);

            Locations = nonduplicitSymbols
                .OfType<BcsCompartmentSymbol>()
                .ToDictionary(k => k.Name);

            LocationEntityMap = nonduplicitSymbols.OfType<BcsComposedSymbol>().ToLocationSymbolMap();

            isInitialized = true;

        }

        private async Task<IList<ApiEntity>> GetEntitiesAsync(int[] ids)
        {
            var list = new List<ApiEntity>();
            foreach (var id in ids.Split(1000))
            {
                var results = await entityRepositry.GetByIdsAsync(id) ?? new ApiEntity[] { };
                foreach (var item in results)
                {
                    list.Add(item);
                }
            }
            return list;
        }
    }
}
