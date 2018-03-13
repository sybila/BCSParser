using System;
using System.Collections.Generic;
using BcsResolver.SemanticModel.Tree;
using BcsAdmin.DAL.Models;
using System.Linq;
using BcsResolver.Extensions;
using System.Collections.Concurrent;

namespace BcsAdmin.BL.Services
{
    public class ReferenceSymbolFactory : SymbolFactory
    {
        private ConcurrentDictionary<string, BcsLocationSymbol> locationMap = new ConcurrentDictionary<string, BcsLocationSymbol>();

        protected override BcsLocationSymbol CreateLocation(EpEntity entity)
        {
            return locationMap.GetOrAdd(entity.Code ?? "", name => base.CreateLocation(entity));
        }
    }

    public class SymbolFactory
    {
        public virtual BcsNamedSymbol CreateSymbol(EpEntity entity)
        {
            switch (entity.HierarchyType)
            {
                case HierarchyType.State:
                    return CreateState(entity);
                case HierarchyType.Compartment:
                    return CreateLocation(entity);
                case HierarchyType.Complex:
                    return CreateComplex(entity);
                case HierarchyType.Structure:
                    return CreateStructuralAgent(entity);
                case HierarchyType.Atomic:
                    return CreateAtomicAgent(entity);
                default:
                    throw new NotSupportedException("HierarchyType is not supported");
            };
        }

        protected virtual BcsStateSymbol CreateState(EpEntity entity)
        {
            return new BcsStateSymbol
            {
                FullName = entity.Name,
                Name = entity.Code,
            };
        }

        protected virtual BcsLocationSymbol CreateLocation(EpEntity entity)
        {
            return new BcsLocationSymbol
            {
                FullName = entity.Name,
                Name = entity.Code,
            };
        }

        protected virtual BcsAtomicAgentSymbol CreateAtomicAgent(EpEntity entity)
        {
            return new BcsAtomicAgentSymbol
            {
                FullName = entity.Name,
                Name = entity.Code,
                Locations = CreateEntityLocations(entity),
                Parts = entity.Children.Select(s => CreateSymbol<BcsStateSymbol>(s).CastTo<BcsNamedSymbol>()).ToList(),
                BcsSymbolType = BcsSymbolType.Agent
            };
        }

        protected virtual BcsStructuralAgentSymbol CreateStructuralAgent(EpEntity entity)
        {
            return new BcsStructuralAgentSymbol
            {
                FullName = entity.Name,
                Name = entity.Code,
                Locations = CreateEntityLocations(entity),
                Parts = entity.Components.Select(s => CreateSymbol<BcsAtomicAgentSymbol>(s.Component).CastTo<BcsNamedSymbol>()).ToList(),
                BcsSymbolType = BcsSymbolType.StructuralAgent
            };
        }

        protected virtual BcsComplexSymbol CreateComplex(EpEntity entity)
        {
            return new BcsComplexSymbol
            {
                FullName = entity.Name,
                Name = entity.Code,
                Locations = CreateEntityLocations(entity),
                Parts = entity.Components.Select(s => CreateSymbol(s.Component).CastTo<BcsNamedSymbol>()).ToList(),
                BcsSymbolType = BcsSymbolType.Complex
            };
        }

        protected virtual List<BcsNamedSymbol> CreateEntityLocations(EpEntity entity)
        {
            return entity.Locations.Select(el => CreateSymbol<BcsLocationSymbol>(el.Location)).ToList();
        }

        protected virtual BcsNamedSymbol CreateSymbol<TExpectedSymbol>(EpEntity entity)
            where TExpectedSymbol : BcsNamedSymbol
        {
            var symbol = CreateSymbol(entity);
            try
            {
                return EnsureType<TExpectedSymbol>(symbol).CastTo<TExpectedSymbol>();
            }
            catch (Exception ex)
            {
                return CreateError(entity, ex);
            }
        }

        private BcsNamedSymbol EnsureType<TSymbol>(BcsNamedSymbol namedSymbol)
            where TSymbol : BcsNamedSymbol
        {
            if (!namedSymbol.Is<TSymbol>())
            {
                throw new InvalidOperationException($"{typeof(TSymbol).FullName} was expected.");
            }
            return namedSymbol;
        }

        protected BcsErrorSymbol CreateError(EpEntity entity, Exception ex)
        {
            return new BcsErrorSymbol
            {
                Error = ex.Message,
                ExpectedType = BcsSymbolType.Location,
                Name = entity.Code
            };
        }
    }
}
