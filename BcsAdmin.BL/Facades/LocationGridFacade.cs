using System;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Queries;
using AutoMapper;
using BcsAdmin.DAL.Api;
using BcsAdmin.BL.Dto;

namespace BcsAdmin.BL.Facades
{
    public class LocationGridFacade : DependantLinkGridFacade<ApiEntity, ApiEntity, LocationLinkDto>
    {
        public LocationGridFacade(
            IRepository<ApiEntity, int> parentRepository, 
            IRepository<ApiEntity, int> associatedEntityRepository,
            Func<ManyToManyQuery<ApiEntity, ApiEntity, LocationLinkDto>> queryFactory,
            IUnitOfWorkProvider unitOfWorkProvider,
            IMapper mapper) 
            : base(parentRepository, associatedEntityRepository, queryFactory, unitOfWorkProvider, mapper)
        {
        }

        protected override void UnlinkCore(ApiEntity parentEntity, int associatedId)
        {
            parentEntity.Compartments.Remove(associatedId);
            ClearAll(parentEntity);
        }

        internal override void LinkCore(ApiEntity parentEntity, int associatedId)
        {
            parentEntity.Compartments.Add(associatedId);
            ClearAll(parentEntity);
        }

        private static void ClearAll(ApiEntity parentEntity)
        {
            parentEntity.Annotations = null;
            parentEntity.Children = null;
            parentEntity.Code = null;
            parentEntity.Description = null;
            parentEntity.Name = null;
            parentEntity.Organisms = null;
            parentEntity.Parent = null;
            parentEntity.Parents = null;
            parentEntity.States = null;
            parentEntity.Status = null;
            parentEntity.Type = null;
            parentEntity.Classifications = null;
        }
    }
}
