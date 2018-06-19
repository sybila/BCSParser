using System;
using System.Collections.Generic;
using BcsResolver.SemanticModel.Tree;
using BcsAdmin.DAL.Models;
using System.Linq;
using System.Collections.Concurrent;
using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.Core;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Services
{
    public class SymbolFactory
    {
        private readonly Func<int, ApiEntity> entityProvider;

        public SymbolFactory(Func<int, ApiEntity> entityProvider)
        {
            this.entityProvider = entityProvider;
        }

        public virtual BcsNamedSymbol CreateSymbol(int id)
        {
            var entity = entityProvider(id);

            if(entity == null)
            {
                return CreateError("<null>", new Exception("Entity not found."));
            }

            switch (entity.Type)
            {
                case ApiEntityType.Compartment:
                    return CreateLocation(entity);
                case ApiEntityType.Complex:
                    return CreateComplex(entity);
                case ApiEntityType.Structure:
                    return  CreateStructuralAgent(entity);
                case ApiEntityType.Atomic:
                    return CreateAtomicAgent(entity);
                default:
                    return CreateError(entity.Name, new NotSupportedException($"Entity of type {entity.Type} cannot be used as part of an agent."));
            };
        }

        protected virtual BcsStateSymbol CreateState(ApiState entity)
        {
            return new BcsStateSymbol
            {
                FullName = entity.Description,
                Name = entity.Code,
            };
        }

        protected virtual BcsCompartmentSymbol CreateLocation(ApiEntity entity)
        {
            return new BcsCompartmentSymbol
            {
                FullName = entity.Name,
                Name = entity.Code,
            };
        }

        protected virtual BcsAtomicAgentSymbol CreateAtomicAgent(ApiEntity entity)
        {
            return new BcsAtomicAgentSymbol
            {
                FullName = entity.Name,
                Name = entity.Code,
                Locations = CreateCompartment(entity),
                Parts = entity.States.Select(s => CreateState(s).CastTo<BcsNamedSymbol>()).ToList(),
                BcsSymbolType = BcsSymbolType.Agent
            };
        }

        protected virtual BcsStructuralAgentSymbol CreateStructuralAgent(ApiEntity entity)
        {
            return new BcsStructuralAgentSymbol
            {
                FullName = entity.Name,
                Name = entity.Code,
                Locations = CreateCompartment(entity),
                Parts = CreateParts<BcsAtomicAgentSymbol>(entity),
                BcsSymbolType = BcsSymbolType.StructuralAgent
            };
        }

        protected virtual BcsComplexSymbol CreateComplex(ApiEntity entity)
        {
            return new BcsComplexSymbol
            {
                FullName = entity.Name,
                Name = entity.Code,
                Locations = CreateCompartment(entity),
                Parts = CreateParts(entity),
                BcsSymbolType = BcsSymbolType.Complex
            };
        }

        private List<BcsNamedSymbol> CreateParts(ApiEntity entity)
            => entity.Children
            .Select(CreateSymbol)
            .ToList();

        private List<BcsNamedSymbol> CreateParts<TPart>(ApiEntity entity)
            where TPart : BcsNamedSymbol
            => entity.Children
            .Select(CreateSymbol<TPart>)
            .ToList();

        private List<BcsNamedSymbol> CreateCompartment(ApiEntity entity) 
            => entity.Compartments
            .Select(CreateSymbol<BcsCompartmentSymbol>)
            .ToList();


        protected virtual BcsNamedSymbol CreateSymbol<TExpectedSymbol>(int id)
            where TExpectedSymbol : BcsNamedSymbol
        {
            var symbol = CreateSymbol(id);
            try
            {
                return EnsureType<TExpectedSymbol>(symbol).CastTo<TExpectedSymbol>();
            }
            catch (Exception ex)
            {
                return CreateError(symbol.Name, ex);
            }
        }

        private BcsNamedSymbol EnsureType<TSymbol>(BcsNamedSymbol namedSymbol)
            where TSymbol : BcsNamedSymbol
        {
            if (namedSymbol is TSymbol)
            {
                return namedSymbol;
            }
            throw new InvalidOperationException($"{typeof(TSymbol).FullName} was expected.");
        }

        protected BcsErrorSymbol CreateError(string entityName, Exception ex)
        {
            return new BcsErrorSymbol
            {
                Error = ex.Message,
                Name = entityName
            };
        }
    }
}
