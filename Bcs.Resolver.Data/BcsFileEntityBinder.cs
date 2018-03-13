using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BcsResolver.Extensions;
using BcsResolver.File;
using BcsResolver.SemanticModel.Exceptions;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.SemanticModel;

namespace BcsResolver.File
{
    public class BcsFileEntityBinder
    {
        private readonly IBcsEntityMetadataProvider metadataProvider;

        private readonly ConcurrentDictionary<string, BcsNamedSymbol> resolvedSymbols = new ConcurrentDictionary<string, BcsNamedSymbol>();

        public BcsFileEntityBinder(IBcsEntityMetadataProvider metadataProvider)
        {
            this.metadataProvider = metadataProvider;
        }

        public IEnumerable<BcsNamedSymbol> BindEntities()
        {
            var entityIdsToProccess = metadataProvider.GetAvailableEntityIds();

            foreach (var entity in entityIdsToProccess)
            {
                yield return BindEntity(entity);
            }

        }

        private BcsNamedSymbol BindEntity(string entity, BcsSymbolType expectedType = BcsSymbolType.Unknown)
        {
            return resolvedSymbols.GetOrAdd(entity, id => BindEntityCore(id, expectedType));
        }

        private BcsNamedSymbol BindEntityCore(string entity, BcsSymbolType expectedType)
        {
            try
            {
                switch (expectedType)
                {
                    case BcsSymbolType.Agent:
                        {
                            var e = EnsureEntity(entity, expectedType);
                            return BindAgent(e.Id);
                        }
                    case BcsSymbolType.StructuralAgent:
                        {
                            var e = EnsureEntity(entity, expectedType);
                            return BindComponet(e.Id);
                        }
                    case BcsSymbolType.Complex:
                        {
                            var e = EnsureEntity(entity, expectedType);
                            return BindCompex(e.Id);
                        }
                    case BcsSymbolType.Unknown:
                    {
                        var e = EnsureEntity(entity, expectedType);
                        return BindByEntityType(e);
                    }
                    case BcsSymbolType.State:
                        return BindState(entity);
                    case BcsSymbolType.Location:
                        return BindLocation(entity);
                    default:
                        throw new UnsupportedEntityException();


                }
            }
            catch (EntityTypeException ex)
            {
                return new BcsErrorSymbol
                {
                    Name = entity,
                    Error = $"Entity type missmatch: {ex.Message}",
                    ExpectedType = expectedType
                };
            }
            catch (EntityNotFoundException ex)
            {
                return new BcsErrorSymbol
                {
                    Name = entity,
                    Error = $"Entity not found: {ex.Message}",
                    ExpectedType = expectedType
                };
            }
            catch (UnsupportedEntityException ex)
            {
                return new BcsErrorSymbol
                {
                    Name = entity,
                    Error = ex.Message,
                    ExpectedType = expectedType
                };
            }
        }

        private BcsNamedSymbol BindByEntityType(BcsEntity e)
        {
            switch (e.Type)
            {
                case BcsEntityType.Agent:
                    return BindAgent(e.Id);
                case BcsEntityType.Component:
                    return BindComponet(e.Id);
                case BcsEntityType.Complex:
                    return BindCompex(e.Id);
                case BcsEntityType.Unknown:
                default:
                    throw new UnsupportedEntityException();
            }
        }

        private BcsEntity EnsureEntity(string entity, BcsSymbolType expectedType)
        {
            var bcsEntity = metadataProvider
                .GetEntity(entity)
                .ThrowIfNull(entity);

            if (expectedType != BcsSymbolType.Unknown)
            {
                bcsEntity.ThrowIfNotType(expectedType);
            }
            return bcsEntity.ThrowIfIdEmpty();
        }

        private static BcsStateSymbol BindState(string entity)
        {
            return new BcsStateSymbol { Name = entity };
        }


        private BcsNamedSymbol BindComponet(string entity)
        {
            var bcsEntity = EnsureEntityId(entity);

            var parts = bcsEntity.Composition
                .Select(id => BindEntity(id, BcsSymbolType.Agent))
                .ToList();

            var location = bcsEntity.Locations
                .Select(BindLocation)
                .OfType<BcsNamedSymbol>()
                .ToList();
            return new BcsStructuralAgentSymbol
            {
                Name = entity,
                Parts = parts,
                Locations = location
            };
        }

        private BcsNamedSymbol BindCompex(string entity)
        {
            var bcsEntity = EnsureEntityId(entity);

            var parts = bcsEntity.Composition
                .Select(id => BindEntity(id, BcsSymbolType.StructuralAgent))
                .ToList();

            var location = bcsEntity.Locations
                .Select(BindLocation)
                .OfType<BcsNamedSymbol>()
                .ToList();

            return new BcsComplexSymbol()
            {
                Name = entity,
                Parts = parts,
                Locations = location
            };
        }

        private BcsNamedSymbol BindAgent(string entity)
        {
            var bcsEntity = EnsureEntityId(entity);

            var parts = bcsEntity.States
                .Select(id => BindEntity(id, BcsSymbolType.State))
                .ToList();
            var location = bcsEntity.Locations
                .Select(BindLocation)
                .OfType<BcsNamedSymbol>()
                .ToList();

            return new BcsAtomicAgentSymbol
            {
                Name = entity,
                Parts = parts,
                Locations = location
            };
        }

        private BcsLocationSymbol BindLocation(string entity)
        {
            //Todo another entity chceck
            return resolvedSymbols.GetOrAdd(entity, new BcsLocationSymbol { Name = entity }) as BcsLocationSymbol;
        }

        private BcsEntity EnsureEntityId(string entity) => metadataProvider.GetEntity(entity);
    }
}
