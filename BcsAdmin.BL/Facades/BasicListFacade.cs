using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Facades
{
    public class BasicListFacade : FacadeBase
    {
        private readonly Func<EntityTypeNamesQuery> entityTypeQueryFunc;

        public BasicListFacade(IUnitOfWorkProvider unitOfWorkProvider, Func<EntityTypeNamesQuery> entityTypeQueryFunc)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
            this.entityTypeQueryFunc = entityTypeQueryFunc;
        }

        public async Task<IList<string>> GetEntityTypeNames()
        {
            return new List<string>() {
                "Compartment",
                "Atomic",
                "Structure",
                "Complex"};
        }

        public List<BiochemicalEntityTypeDto> GetEntityTypes()
        {
            return
                Enum.GetValues(typeof(Dto.HierarchyType))
                .Cast<Dto.HierarchyType>()
                .Select(CreateHierarchyTypeDto)
                .ToList();
        }

        private static BiochemicalEntityTypeDto CreateHierarchyTypeDto(Dto.HierarchyType v)
        {
            return new BiochemicalEntityTypeDto
            {
                Id = (int)v,
                Name = v.ToString("F")
            };
        }

        public List<BiochemicalEntityTypeDto> GetEntityTypesForParentType(int parentTypeId)
        {
            var t = (Dto.HierarchyType)parentTypeId;

            return GetEntityTypesForParentTypeCore(t).ToList();
        }

        private static IEnumerable<BiochemicalEntityTypeDto> GetEntityTypesForParentTypeCore(Dto.HierarchyType t)
        {
            switch (t)
            {
                case Dto.HierarchyType.State:
                    yield break;
                case Dto.HierarchyType.Compartment:
                    yield break;
                case Dto.HierarchyType.Complex:
                    yield return CreateHierarchyTypeDto(Dto.HierarchyType.Atomic);
                    yield return CreateHierarchyTypeDto(Dto.HierarchyType.Structure);
                    yield return CreateHierarchyTypeDto(Dto.HierarchyType.Complex);
                    yield break;
                case Dto.HierarchyType.Structure:
                    yield return CreateHierarchyTypeDto(Dto.HierarchyType.Atomic);
                    break;
                case Dto.HierarchyType.Atomic:
                    yield return CreateHierarchyTypeDto(Dto.HierarchyType.State);
                    break;
                default:
                    yield break;
            }
        }
    }
}
