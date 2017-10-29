using BcsResolver.File;
using System;
using System.Collections.Generic;
using System.Text;
using BcsResolver.SemanticModel.Tree;
using BcsAdmin.DAL.Models;

namespace BcsAdmin.BL.Services
{
    class DbWorkspace : IBcsWorkspace, IDisposable
    {
        private readonly EcyanoNewDbContext dbContext;

        public IReadOnlyDictionary<string, BcsComplexSymbol> Complexes => throw new NotImplementedException();

        public IReadOnlyDictionary<string, BcsStructuralAgentSymbol> StructuralAgents => throw new NotImplementedException();

        public IReadOnlyDictionary<string, BcsAtomicAgentSymbol> AtomicAgents => throw new NotImplementedException();

        public IReadOnlyDictionary<string, BcsLocationSymbol> Locations => throw new NotImplementedException();

        public IReadOnlyDictionary<string, IReadOnlyList<BcsComposedSymbol>> LocationEntityMap => throw new NotImplementedException();

        public DbWorkspace(EcyanoNewDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateSemanticModel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
