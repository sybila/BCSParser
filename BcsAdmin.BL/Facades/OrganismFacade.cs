using System;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.DAL.Models;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using AutoMapper;

namespace BcsAdmin.BL.Facades
{
    public class OrganismFacade : DependantLinkGridFacade<EpEntityOrganism, EpOrganism, EntityOrganismDto>
    {
        public OrganismFacade(IRepository<EpEntityOrganism, int> intermediateRepository,
            IRepository<EpOrganism, int> associatedEntityRepository,
            Func<IdFilteredQuery<EntityOrganismDto>> queryFactory,
            IUnitOfWorkProvider unitOfWorkProvider, IMapper mapper)
            : base(intermediateRepository, associatedEntityRepository, queryFactory, unitOfWorkProvider, mapper)
        {
        }
    }
}
