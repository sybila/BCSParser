using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BcsAdmin.BL.Queries
{
    public class OrganismQuery : ManyToManyQuery<ApiEntity, ApiOrganism, EntityOrganismDto>
    {
        public OrganismQuery(IRepository<ApiEntity, int> parentEntityRepository, 
            IRepository<ApiOrganism, int> associatedEntityRepository) 
            : base(parentEntityRepository, associatedEntityRepository)
        {
        }

        protected override IList<int> GetAssocitedEntityIds(ApiEntity parent)
        {
            return parent.Organisms;
        }

        protected override IQueryable<EntityOrganismDto> ProcessEntities(IQueryable<ApiOrganism> q, ApiEntity parentEntity)
        {
            return q.Select(e => new EntityOrganismDto
             {
                 Id = e.Id,
                 Name = e.Name,
                 Code = "",
                 GeneGroup = "",
                 IntermediateEntityId = parentEntity.Id
             });
        }
    }
}
