using System;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.DAL.Api;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using AutoMapper;
using System.Collections.Generic;
using BcsAdmin.BL.Repositories.Api;

namespace BcsAdmin.BL.Facades
{
    public class OrganismFacade: DependantLinkGridFacade<OrganismArray, ApiOrganism, OrganismDto>
    {
        private readonly Func<IRepository<OrganismArray, int>> parentRepositoryFunc;

        public OrganismFacade(
            Func<IRepository<OrganismArray, int>> parentRepositoryFunc, 
            IRepository<ApiOrganism, int> associatedEntityRepository, 
            Func<ManyToManyQuery<OrganismArray, ApiOrganism, OrganismDto>> queryFactory, 
            IUnitOfWorkProvider unitOfWorkProvider, 
            IMapper mapper) 
            : base(associatedEntityRepository, queryFactory, mapper)
        {
            this.parentRepositoryFunc = parentRepositoryFunc;
        }

        protected override IRepository<OrganismArray, int> GetParentRepository(string paentRepositoryName)
        {
            var r = (OrganismArrayRepository)parentRepositoryFunc();
            r.RepoName = paentRepositoryName;
            return r;
        }

        protected override void UnlinkCore(OrganismArray parentEntity, int associatedId)
        {
            parentEntity.Organisms.Remove(associatedId);
        }

        internal override void LinkCore(OrganismArray parentEntity, int associatedId)
        {
            parentEntity.Organisms.Add(associatedId);
        }
    }
}
