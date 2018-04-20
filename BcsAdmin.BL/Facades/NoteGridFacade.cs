using System;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.DAL.Models;
using DotVVM.Framework.Controls;
using System.Threading.Tasks;

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
}
