using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BcsAdmin.BL.Queries
{
    public class LocationLinkQuery : ManyToManyQuery<ApiEntity, ApiEntity, LocationLinkDto>
    {
        private readonly IRepository<ApiEntity, int> parentEntityRepository;

        public LocationLinkQuery(
            IRepository<ApiEntity, int> parentEntityRepository,
            IRepository<ApiEntity, int> associatedEntityRepository) 
            : base(associatedEntityRepository)
        {
            this.parentEntityRepository = parentEntityRepository;
        }

        protected override IList<int> GetAssocitedEntityIds(ApiEntity parent)
        {
            return parent.Compartments;
        }

        protected override IRepository<ApiEntity, int> GetParentRepository() => parentEntityRepository;

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
