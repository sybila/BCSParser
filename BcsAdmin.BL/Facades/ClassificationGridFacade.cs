using System;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.DAL.Api;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using AutoMapper;

namespace BcsAdmin.BL.Facades
{
    public class ClassificationGridFacade : DependantLinkGridFacade<ApiEntity, ApiClassification, ClassificationDto>
    {
        public ClassificationGridFacade(
            IRepository<ApiEntity, int> parentRepository,
            IRepository<ApiClassification, int> associatedEntityRepository, 
            Func<ManyToManyQuery<ApiEntity, ApiClassification, ClassificationDto>> queryFactory, 
            IUnitOfWorkProvider unitOfWorkProvider, 
            IMapper mapper) 
            : base(parentRepository, associatedEntityRepository, queryFactory, unitOfWorkProvider, mapper)
        {
        }

        protected override void UnlinkCore(ApiEntity parentEntity, int associatedId)
        {
            parentEntity.Classifications.Remove(associatedId);
            ClearAll(parentEntity);
        }

        internal override void LinkCore(ApiEntity parentEntity, int associatedId)
        {
            parentEntity.Classifications.Add(associatedId);
            ClearAll(parentEntity);
        }

        private static void ClearAll(ApiEntity parentEntity)
        {
            parentEntity.Code = null;
            parentEntity.Description = null;
            parentEntity.Name = null;           
            parentEntity.Parent = null;
            parentEntity.Parents = null;
            parentEntity.Status = null;
            parentEntity.Type = null;


            parentEntity.Children = null;
            parentEntity.Annotations = null;
            parentEntity.Compartments = null;
            parentEntity.States = null;
            parentEntity.Organisms = null;
        }
    }
}
