using BcsAdmin.DAL.Models;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;

namespace BcsAdmin.BL.Queries
{
    public class OrganismQuery : IdFilteredQuery<EntityOrganismDto>
    {
        public OrganismQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<EntityOrganismDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();
            context.EpEntity.Load();
            context.EpOrganism.Load();
            return context.EpEntityOrganism.Where(e => e.EntityId == Filter.Id).Select(e => new EntityOrganismDto
            {
                Id = e.Organism.Id,
                Name = e.Organism.Name,
                Code = e.Organism.Code,
                GeneGroup = e.GeneGroup,
                IntermediateEntityId = e.Id
            });
        }
    }
}
