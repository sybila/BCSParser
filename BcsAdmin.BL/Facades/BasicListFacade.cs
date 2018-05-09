using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using BcsAdmin.DAL.Api;
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

        public List<AnnotationTypeDto> GetAnnotationTypes()
        {
            return new List<AnnotationTypeDto>() {
                new AnnotationTypeDto { Id = 0, Name = "bnid" },
                new AnnotationTypeDto { Id = 1, Name = "chebi"},
                new AnnotationTypeDto { Id = 2, Name = "doi"},
                new AnnotationTypeDto { Id = 3, Name = "ec-code"},
                new AnnotationTypeDto { Id = 4, Name = "go"},
                new AnnotationTypeDto { Id = 5, Name = "kegg"},
                new AnnotationTypeDto { Id = 6, Name = "pubchem"},
                new AnnotationTypeDto { Id = 7, Name = "uniprot"},
                new AnnotationTypeDto { Id = 8, Name = "url"},
                new AnnotationTypeDto { Id = 9, Name = "ncbi"},
            };
        }

        public List<BiochemicalEntityTypeDto> GetEntityTypes()
        {
            return
                Enum.GetValues(typeof(Dto.HierarchyType))
                .Cast<Dto.HierarchyType>()
                .Select(CreateHierarchyTypeDto)
                .ToList();
        }

        public List<StatusDto> GetBcsObjectStatuses()
        {
            return new List<StatusDto> {
                new StatusDto { Id = (int)ApiEntityStatus.Active, Name = ApiEntityStatus.Active.ToString("F")},
                new StatusDto { Id = (int)ApiEntityStatus.Pending, Name = ApiEntityStatus.Pending.ToString("F")},
                new StatusDto { Id = (int)ApiEntityStatus.Inactive, Name = ApiEntityStatus.Inactive.ToString("F")}
            };             
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
                default:
                    yield break;
            }
        }
    }
}
