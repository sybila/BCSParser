using System;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.DAL.Api;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using AutoMapper;

namespace BcsAdmin.BL.Facades
{
    public class OrganismFacade : DependantLinkGridFacade<ApiEntity, ApiOrganism, EntityOrganismDto>
    {
        public OrganismFacade(
            IRepository<ApiEntity, int> parentRepository, 
            IRepository<ApiOrganism, int> associatedEntityRepository, 
            Func<ManyToManyQuery<ApiEntity, ApiOrganism, EntityOrganismDto>> queryFactory, 
            IUnitOfWorkProvider unitOfWorkProvider, 
            IMapper mapper) 
            : base(parentRepository, associatedEntityRepository, queryFactory, unitOfWorkProvider, mapper)
        {
        }

        protected override void UnlinkCore(ApiEntity parentEntity, int associatedId)
        {
            parentEntity.Organisms.Remove(associatedId);
        }

        internal override void LinkCore(ApiEntity parentEntity, int associatedId)
        {
            parentEntity.Organisms.Add(associatedId);
        }
    }
}
