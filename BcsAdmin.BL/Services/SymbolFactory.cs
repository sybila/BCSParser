using System;
using System.Collections.Generic;
using BcsResolver.SemanticModel.Tree;
using BcsAdmin.DAL.Models;
using System.Linq;
using BcsResolver.Extensions;

namespace BcsAdmin.BL.Services
{
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
                Locations = entity.Locations.Select(el => CreateLocation(el.ParentEntity)).ToList(),
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
                Parts = entity.Components.Select(s => CreateSymbol<BcsAtomicAgentSymbol>(s.ChildEntity).CastTo<BcsNamedSymbol>()).ToList(),
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
                Parts = entity.Components.Select(s => CreateSymbol(s.ChildEntity).CastTo<BcsNamedSymbol>()).ToList(),
                BcsSymbolType = BcsSymbolType.Complex
            };
        }

        protected virtual List<BcsLocationSymbol> CreateEntityLocations(EpEntity entity)
        {
            return entity.Locations.Select(el => CreateSymbol<BcsLocationSymbol>(el.ParentEntity)).ToList();
        }

        protected virtual TExpectedSymbol CreateSymbol<TExpectedSymbol>(EpEntity entity)
            where TExpectedSymbol : BcsNamedSymbol
        {
            var symbol = CreateSymbol(entity);
            return EnsureType<TExpectedSymbol>(symbol).CastTo<TExpectedSymbol>();
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
    }
}
