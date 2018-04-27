using System;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.DAL.Models;
using DotVVM.Framework.Controls;
using System.Threading.Tasks;
using BcsAdmin.BL.Facades.Defs;
using BcsAdmin.BL.Repositories.Api;
using BcsAdmin.DAL.Api;

namespace BcsAdmin.BL.Facades
{
    public class NoteGridFacade : IGridFacade<int, EntityNoteDto>
    {
        private readonly Func<IRepository<ApiNote, int>> repositoryFactory;
        private readonly IEntityDTOMapper<ApiNote, EntityNoteDto> mapper;

        public Func<IFilteredQuery<EntityNoteDto, IdFilter>> QueryFactory { get; }

        public NoteGridFacade(
            Func<IFilteredQuery<EntityNoteDto, IdFilter>> queryFactory,
            Func<IRepository<ApiNote, int>> repositoryFactory,
            IEntityDTOMapper<ApiNote, EntityNoteDto> mapper)
        {
            QueryFactory = queryFactory;
            this.repositoryFactory = repositoryFactory;
            this.mapper = mapper;
        }
      
        public void Delete(int parentId, int id)
        {
            var r = GetRepository(parentId);
            r.Delete(id);
        }

        public EntityNoteDto GetDetail(int parentId, int id)
        {
            var r = GetRepository(parentId);
            var apiEntity = r.GetById(id);

            return mapper.MapToDTO(apiEntity);            
        }

        public EntityNoteDto InitializeNew()
        {
            return new EntityNoteDto
            {

            };
        }

        public EntityNoteDto Save(int parentId, EntityNoteDto data)
        {
            var r = GetRepository(parentId);

            var apiEntity = mapper.MapToEntity(data);

            if (data.Id == 0) {
                r.Insert(apiEntity);
            }
            else
            {
                r.Update(apiEntity);
            }
            return mapper.MapToDTO(apiEntity);
        }

        public async Task FillDataSetAsync(GridViewDataSet<EntityNoteDto> dataSet, IdFilter filter)
        {
            var q = QueryFactory();
            q.Filter = filter;
            await dataSet.LoadFromQueryAsync(q);
        }

        private ApiNoteRepository GetRepository(int parentId)
        {
            var r = repositoryFactory().CastAs<ApiNoteRepository>();
            r.RepoName = $"entities/{parentId}/notes";
            return r;
        }
    }
}
