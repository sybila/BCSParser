using System;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.DAL.Models;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;

namespace BcsAdmin.BL.Facades
{
    public class NoteGridFacade : FilteredCrudFacadeBase<EpEntityNote, int, EntityNoteDto, EntityNoteDto, IdFilter>, IGridFacade<EntityNoteDto>
    {
        public NoteGridFacade(
            IUnitOfWorkProvider unitOfWorkProvider,
            Func<IFilteredQuery<EntityNoteDto, IdFilter>> queryFactory,
            IRepository<EpEntityNote, int> repository,
            IEntityDTOMapper<EpEntityNote, EntityNoteDto> mapper)
            : base(queryFactory, repository, mapper)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
        }
    }

    public class StateGridFacade : FilteredCrudFacadeBase<EpEntity, int, StateEntityDto, StateEntityDto, IdFilter>, IGridFacade<StateEntityDto>
    {
        public StateGridFacade(
            IUnitOfWorkProvider unitOfWorkProvider,
            Func<IFilteredQuery<StateEntityDto, IdFilter>> queryFactory, 
            IRepository<EpEntity, int> repository, 
            IEntityDTOMapper<EpEntity, StateEntityDto> mapper) 
            : base(queryFactory, repository, mapper)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
        }
    }
}
