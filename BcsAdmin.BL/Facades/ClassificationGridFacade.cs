using System;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.DAL.Models;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using AutoMapper;

namespace BcsAdmin.BL.Facades
{
    public class ClassificationGridFacade : DependantLinkGridFacade<EpEntityClassification, EpClassification, ClassificationDto>
    {
        public ClassificationGridFacade(
            IRepository<EpEntityClassification, int> intermediateRepository,
            IRepository<EpClassification, int> associatedEntityRepository,
            Func<IdFilteredQuery<ClassificationDto>> queryFactory,
            IUnitOfWorkProvider unitOfWorkProvider, IMapper mapper)
            : base(intermediateRepository, associatedEntityRepository, queryFactory, unitOfWorkProvider, mapper)
        {
        }
    }
}
