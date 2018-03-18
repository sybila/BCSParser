using BcsAdmin.DAL.Models;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;

namespace BcsAdmin.BL.Queries
{
    public class ComponentLinkQuery : IdFilteredQuery<ComponentLinkDto>
    {
        public ComponentLinkQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<ComponentLinkDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();
            context.EpEntity.Load();
            context.EpEntityComposition.Load();

            var parentEntity = context.EpEntity.Find(Filter.Id);

            if (parentEntity == null) { return Enumerable.Empty<ComponentLinkDto>().AsQueryable(); }

            return context.EpEntityComposition.Where(e => e.ComposedEntity.Id == Filter.Id).Select(e => new ComponentLinkDto
            {
                Id = e.Component.Id,
                Code = e.Component.Code,
                HierarchyType = (int)e.Component.HierarchyType,
                Name = e.Component.Name,
                IntermediateEntityId = e.Id
            });
        }
    }
}
