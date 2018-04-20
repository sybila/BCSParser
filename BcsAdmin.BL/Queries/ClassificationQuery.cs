using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BcsAdmin.BL.Queries
{
    public class ClassificationQuery : ManyToManyQuery<ApiEntity, ApiClassification, ClassificationDto>
    {
        public ClassificationQuery(IRepository<ApiEntity, int> parentEntityRepository, IRepository<ApiClassification, int> associatedEntityRepository)
            : base(parentEntityRepository, associatedEntityRepository)
        {
        }

        protected override IQueryable<ClassificationDto> ProcessEntities(IQueryable<ApiClassification> q, ApiEntity parentEntity)
        {
            return q
                .Where(e => e.Id == Filter.Id)
                .Select(e => new ClassificationDto
            {
                Id = e.Id,
                Name = e.Name,
                Type = e.Type,
                IntermediateEntityId = parentEntity.Id
            });
        }

        protected override IList<int> GetAssocitedEntityIds(ApiEntity parent)
        {
            return parent.Classifications;
        }
    }
}
