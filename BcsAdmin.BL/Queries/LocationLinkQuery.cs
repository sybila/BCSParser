using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BcsAdmin.BL.Repositories.Api;

namespace BcsAdmin.BL.Queries
{
    public class LocationLinkQuery : ManyToManyQuery<ApiEntity, ApiEntity, LocationLinkDto>
    {
        private readonly IAsyncRepository<ApiEntity, int> parentEntityRepository;

        public LocationLinkQuery(
            IAsyncRepository<ApiEntity, int> parentEntityRepository,
            IAsyncRepository<ApiEntity, int> associatedEntityRepository) 
            : base(associatedEntityRepository)
        {
            this.parentEntityRepository = parentEntityRepository;
        }

        protected override IList<int> GetAssocitedEntityIds(ApiEntity parent)
        {
            return parent.Compartments;
        }

        protected override IAsyncRepository<ApiEntity, int> GetParentRepository() => parentEntityRepository;

        protected override IQueryable<LocationLinkDto> ProcessEntities(IQueryable<ApiEntity> q, ApiEntity parentEntity)
        {
            return q.Select(e => new LocationLinkDto
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IntermediateEntityId = parentEntity.Id
            });
        }
    }
}
