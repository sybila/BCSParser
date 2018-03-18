using System;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.DAL.Models;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using AutoMapper;

namespace BcsAdmin.BL.Facades
{
    public class ComponentsGridFacade : DependantLinkGridFacade<EpEntityComposition, EpEntity, ComponentLinkDto>
    {
        public ComponentsGridFacade(IRepository<EpEntityComposition, int> intermediateRepository, IRepository<EpEntity, int> associatedEntityRepository, Func<IdFilteredQuery<ComponentLinkDto>> queryFactory, IUnitOfWorkProvider unitOfWorkProvider, IMapper mapper)
            : base(intermediateRepository, associatedEntityRepository, queryFactory, unitOfWorkProvider, mapper)
        {
        }
    }
}
