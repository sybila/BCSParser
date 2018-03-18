using BcsAdmin.DAL.Models;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;

namespace BcsAdmin.BL.Queries
{
    public class StateEntityQuery : IdFilteredQuery<StateEntityDto>
    {
        public StateEntityQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<StateEntityDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();
            context.EpEntity.Load();
            context.EpEntityComposition.Load();

            var parentEntity = context.EpEntity.Find(Filter.Id);

            if (parentEntity == null) { return Enumerable.Empty<StateEntityDto>().AsQueryable(); }

            return context.EpEntity.Where(e => e.ParentId == parentEntity.Id).Select(e => new StateEntityDto
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IntermediateEntityId = null
            });
        }
    }
}
