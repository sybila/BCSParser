using System;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Repositories.Api;
using BcsAdmin.DAL.Api;

namespace BcsAdmin.BL.Facades
{
    public class NoteGridFacade : DependantGridFacade<ApiNote, EntityNoteDto>
    {
        public NoteGridFacade(Func<IFilteredQuery<EntityNoteDto, IdFilter>> queryFactory, 
            Func<IAsyncRepository<ApiNote, int>> repositoryFactory,
            IEntityDTOMapper<ApiNote, EntityNoteDto> mapper) 
            : base(queryFactory, repositoryFactory, mapper)
        {
        }

        protected override IAsyncRepository<ApiNote, int> GetRepository(int parentId, string parentRepositoryName)
        {
            var r = RepositoryFactory().CastAs<ApiNoteRepository>();
            r.RepoName = $"{parentRepositoryName}/{parentId}/notes";
            return r;
        }

    }
}
