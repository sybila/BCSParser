using System;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.DAL.Models;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using AutoMapper;

namespace BcsAdmin.BL.Facades
{
    public class LocationGridFacade : DependantLinkGridFacade<EpEntityLocation, EpEntity, LocationLinkDto>
    {
        public LocationGridFacade(IRepository<EpEntityLocation, int> intermediateRepository, IRepository<EpEntity, int> associatedEntityRepository, Func<IdFilteredQuery<LocationLinkDto>> queryFactory, IUnitOfWorkProvider unitOfWorkProvider, IMapper mapper) : base(intermediateRepository, associatedEntityRepository, queryFactory, unitOfWorkProvider, mapper)
        {
        }
    }
}
