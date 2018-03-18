using BcsAdmin.DAL.Models;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;

namespace BcsAdmin.BL.Queries
{
    public class ClassificationQuery : IdFilteredQuery<ClassificationDto>
    {
        public ClassificationQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<ClassificationDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();
            context.EpEntity.Load();
            context.EpClassification.Load();
            return context.EpEntityClassification.Where(e => e.Entity.Id == Filter.Id).Select(e => new ClassificationDto
            {
                Id = e.Classification.Id,
                Name = e.Classification.Name,
                Type = e.Classification.Type,
                IntermediateEntityId = e.Id
            });
        }
    }
}
