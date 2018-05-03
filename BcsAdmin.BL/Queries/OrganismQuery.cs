using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BcsAdmin.BL.Queries
{
    public class OrganismQuery : ManyToManyQuery<OrganismArray, ApiOrganism, OrganismDto>
    {
        public OrganismQuery(IRepository<OrganismArray, int> parentEntityRepository, 
            IRepository<ApiOrganism, int> associatedEntityRepository) 
            : base(parentEntityRepository, associatedEntityRepository)
        {
        }

        protected override IList<int> GetAssocitedEntityIds(OrganismArray parent)
        {
            return parent.Organisms;
        }

        protected override IQueryable<OrganismDto> ProcessEntities(IQueryable<ApiOrganism> q, OrganismArray parentEntity)
        {
            return q.Select(e => new OrganismDto
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
