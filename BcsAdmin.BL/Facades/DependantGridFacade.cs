using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.DAL.Models;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Queries;
using BcsAdmin.BL.Repositories;

namespace BcsAdmin.BL.Facades
{
    public class DependantGridFacade<TIntermediateEntity, TEntityDto> : FilteredCrudFacadeBase<TIntermediateEntity, int, TEntityDto, TEntityDto, IdFilter>
        where TIntermediateEntity : IEntity<int>
        where TEntityDto : IEntity<int>
    {
        public DependantGridFacade(
            Func<IdFilteredQuery<TEntityDto>> queryFactory,
            IRepository<TIntermediateEntity, int> repository,
            IEntityDTOMapper<TIntermediateEntity, TEntityDto> mapper,
            IUnitOfWorkProvider unitOfWorkProvider)
            : base(queryFactory, repository, mapper)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
        }
    }

    public class LocationGridFacade : DependantGridFacade<EpEntityLocation, LocationLinkDto>
    {
        public LocationGridFacade(
            Func<LocationLinkQuery> queryFactory,
            EntityLocationRepository repository,
            IEntityDTOMapper<EpEntityLocation, LocationLinkDto> mapper,
            IUnitOfWorkProvider unitOfWorkProvider)
            : base(queryFactory, repository, mapper, unitOfWorkProvider)
        {
        }
    }

    public class ComponentsGridFacade : DependantGridFacade<EpEntityComposition, ComponentLinkDto>
    {
        public ComponentsGridFacade(
            Func<ComponentLinkQuery> queryFactory,
            EntityComponentRepository repository,
            IEntityDTOMapper<EpEntityComposition, ComponentLinkDto> mapper,
            IUnitOfWorkProvider unitOfWorkProvider)
            : base(queryFactory, repository, mapper, unitOfWorkProvider)
        {
        }
    }

    public class NoteGridFacade : DependantGridFacade<EpEntityNote, EntityNoteDto>
    {
        public NoteGridFacade(
            Func<NoteQuery> queryFactory,
            EntityNoteRepository repository,
            IEntityDTOMapper<EpEntityNote, EntityNoteDto> mapper,
            IUnitOfWorkProvider unitOfWorkProvider)
            : base(queryFactory, repository, mapper, unitOfWorkProvider)
        {
        }
    }

    public class ClassificationGridFacade : DependantGridFacade<EpEntityClassification, ClassificationDto>
    {
        public ClassificationGridFacade(
            Func<ClassificationQuery> queryFactory,
            EntityClassificationRepository repository,
            IEntityDTOMapper<EpEntityClassification, ClassificationDto> mapper,
            IUnitOfWorkProvider unitOfWorkProvider)
            : base(queryFactory, repository, mapper, unitOfWorkProvider)
        {
        }
    }
}
