using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace BcsAdmin.BL.Queries
{
    public class ComponentLinkQuery : ManyToManyQuery<ApiEntity, ApiEntity, ComponentLinkDto>
    {
        public ComponentLinkQuery(
            IRepository<ApiEntity, int> parentEntityRepository,
            IRepository<ApiEntity, int> associatedEntityRepository) 
            : base(parentEntityRepository, associatedEntityRepository)
        {
        }

        protected override IList<int> GetAssocitedEntityIds(ApiEntity parent)
        {
            return parent.Children;
        }

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
