using BcsAdmin.BL.Dto;
using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Queries
{
    public class EntityUsageQuery : OneToManyQuery<ApiEntity, EntityUsageDto>
    {
        private readonly IRepository<ApiEntity, int> entityRepository;

        public EntityUsageQuery(IRepository<ApiEntity, int> entityRepository) : base(entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        protected async override Task<IQueryable<EntityUsageDto>> GetQueriableAsync(CancellationToken cancellationToken)
        {
            var entity = await ParentEntityRepository.GetByIdAsync(cancellationToken, Filter.Id);

            var allEntities = await GetWebDataAsync<ApiQueryEntity>(cancellationToken, "entities");

            var parentIds = entity.Parents ?? (
                entity.Parent.HasValue 
                ? new int[] { entity.Parent.Value }
                : new int[] { });

            var q = await entityRepository.GetByIdsAsync(cancellationToken, parentIds);

                return q.Select(e => new EntityUsageDto
                {
                    Id = e.Id,
                    CategoryType = CategoryType.Entity,
                    FullName = $"{e.Code} - {e.Name}"
                }).AsQueryable();
        }
    }
}
