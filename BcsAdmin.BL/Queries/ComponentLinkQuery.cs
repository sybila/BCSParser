using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using BcsAdmin.BL.Repositories.Api;

namespace BcsAdmin.BL.Queries
{
    public class ComponentLinkQuery : ManyToManyQuery<ApiEntity, ApiEntity, ComponentLinkDto>
    {
        private readonly IAsyncRepository<ApiEntity, int> parentEntityRepository;

        public ComponentLinkQuery(
            IAsyncRepository<ApiEntity, int> parentEntityRepository,
            IAsyncRepository<ApiEntity, int> associatedEntityRepository) 
            : base(associatedEntityRepository)
        {
            this.parentEntityRepository = parentEntityRepository;
        }

        protected override IList<int> GetAssocitedEntityIds(ApiEntity parent)
        {
            return parent.Children;
        }

        protected override IAsyncRepository<ApiEntity, int> GetParentRepository() => parentEntityRepository;

        protected override IQueryable<ComponentLinkDto> ProcessEntities(IQueryable<ApiEntity> q, ApiEntity parentEntity)
        {
            return q.Select(e => new ComponentLinkDto
            {
                Id = e.Id,
                Code = e.Code,
                HierarchyType = (int)e.Type,
                Name = e.Name,
                IntermediateEntityId = parentEntity.Id
            });
        }
    }
}
