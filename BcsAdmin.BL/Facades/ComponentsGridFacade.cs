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
        private readonly Func<IRepository<ApiEntity, int>> parentRepositoryFunc;
        public ComponentsGridFacade(
            Func<IRepository<ApiEntity, int>> parentRepositoryFunc,
            IRepository<ApiEntity, int> associatedEntityRepository,
            Func<ManyToManyQuery<ApiEntity, ApiEntity, ComponentLinkDto>> queryFactory,
            IUnitOfWorkProvider unitOfWorkProvider, 
            IMapper mapper) 
            : base(associatedEntityRepository, queryFactory, mapper)
        {
            this.parentRepositoryFunc = parentRepositoryFunc;
        }

        protected override void UnlinkCore(ApiEntity parentEntity, int associatedId)
        {
            parentEntity.Children.Remove(associatedId);
            ClearAll(parentEntity);
        }

        internal override void LinkCore(ApiEntity parentEntity, int associatedId)
        {
            parentEntity.Children.Add(associatedId);
            ClearAll(parentEntity);
        }

        private static void ClearAll(ApiEntity parentEntity)
        {
            parentEntity.Annotations = null;
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
            parentEntity.Compartments = null;
        }

        protected override IRepository<ApiEntity, int> GetParentRepository(string paentRepositoryName) =>
            parentRepositoryFunc();
    }
}
