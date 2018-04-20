using System;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.DAL.Api;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using AutoMapper;

namespace BcsAdmin.BL.Facades
{
    public class ComponentsGridFacade : DependantLinkGridFacade<ApiEntity, ApiEntity, ComponentLinkDto>
    {
        public ComponentsGridFacade(
            IRepository<ApiEntity, int> parentRepository,
            IRepository<ApiEntity, int> associatedEntityRepository,
            Func<ManyToManyQuery<ApiEntity, ApiEntity, ComponentLinkDto>> queryFactory,
            IUnitOfWorkProvider unitOfWorkProvider, 
            IMapper mapper) 
            : base(parentRepository, associatedEntityRepository, queryFactory, unitOfWorkProvider, mapper)
        {
        }

        protected override void UnlinkCore(ApiEntity parentEntity, int associatedId)
        {
            parentEntity.Children.Remove(associatedId);
        }

        internal override void LinkCore(ApiEntity parentEntity, int associatedId)
        {
            parentEntity.Children.Add(associatedId);
        }
    }
}
