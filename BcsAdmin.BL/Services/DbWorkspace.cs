using BcsResolver.File;
using System;
using System.Collections.Generic;
using System.Text;
using BcsResolver.SemanticModel.Tree;
using BcsAdmin.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Bcs.Resolver.Common;

namespace BcsAdmin.BL.Services
{
    public class DbWorkspace : IBcsWorkspace
    {
        private bool isInitialized;

        private readonly SemanticModelFactory semanticModelFactory;

        public IReadOnlyDictionary<string, BcsComplexSymbol> Complexes { get; private set; }

        public IReadOnlyDictionary<string, BcsStructuralAgentSymbol> StructuralAgents { get; private set; }

        public IReadOnlyDictionary<string, BcsAtomicAgentSymbol> AtomicAgents { get; private set; }

        public IReadOnlyDictionary<string, BcsLocationSymbol> Locations { get; private set; }

        public IReadOnlyDictionary<string, IReadOnlyList<BcsComposedSymbol>> LocationEntityMap { get; private set; }

        public DbWorkspace()
        {
            semanticModelFactory = new SemanticModelFactory();
        }

        public IEnumerable<BcsComposedSymbol> GetAllEntities()
           => AtomicAgents.Values
               .Concat(StructuralAgents.Values.Cast<BcsComposedSymbol>())
               .Concat(Complexes.Values);

        public void CreateSemanticModel()
        {
            if(isInitialized)
            {
                return;
            }

            using (var dbContext = new AppDbContext())
            {
                dbContext.EpEntity.Load();
                dbContext.EpEntityComposition.Load();
                dbContext.EpEntityLocation.Load();
                dbContext.EpClassification.Load();
                dbContext.EpEntityLocation.Load();

                var e = dbContext.EpEntity.ToList();

                var symbols = dbContext.EpEntity.Select(semanticModelFactory.CreateSymbol).Distinct().ToList();

                AtomicAgents = symbols
                    .OfType<BcsAtomicAgentSymbol>()
                    .ToDictionary(k => k.Name);

                StructuralAgents = symbols
                    .OfType<BcsStructuralAgentSymbol>()
                    .ToDictionary(k => k.Name);

                Complexes = symbols
                    .OfType<BcsComplexSymbol>()
                    .ToDictionary(k => k.Name);

                Locations = symbols
                    .OfType<BcsLocationSymbol>()
                    .ToDictionary(k => k.Name);

                LocationEntityMap = symbols.OfType<BcsComposedSymbol>().ToLocationSymbolMap();

                isInitialized = true;
            }
        }
    }
}
