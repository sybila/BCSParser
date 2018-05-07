using System;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.BL.Filters;
using BcsAdmin.DAL.Models;
using DotVVM.Framework.Controls;
using System.Threading.Tasks;
using BcsAdmin.BL.Facades.Defs;
using BcsAdmin.BL.Repositories.Api;
using System.Threading;

namespace BcsAdmin.BL.Facades
{
    public abstract class DependantGridFacade<TApiEntity, TEntityDto> : IGridFacade<int, TEntityDto>
        where TApiEntity : IEntity<int>, new()
        where TEntityDto : class, IEntity<int>, new()
    {
        protected Func<IAsyncRepository<TApiEntity, int>> RepositoryFactory { get; }
        protected IEntityDTOMapper<TApiEntity, TEntityDto> Mapper { get; }
        public Func<IFilteredQuery<TEntityDto, IdFilter>> QueryFactory { get; }

        public DependantGridFacade(
            Func<IFilteredQuery<TEntityDto, IdFilter>> queryFactory,
            Func<IAsyncRepository<TApiEntity, int>> repositoryFactory,
            IEntityDTOMapper<TApiEntity, TEntityDto> mapper)
        {
            QueryFactory = queryFactory;
            this.RepositoryFactory = repositoryFactory;
            this.Mapper = mapper;
        }

        public async Task DeleteAsync(int parentId, string parentRepositoryName, int id)
        {
            var r = GetRepository(parentId, parentRepositoryName);
            await r.DeteleAsync(id);
        }

        public async Task<TEntityDto> GetDetailAsync(int parentId, string parentRepositoryName, int id)
        {
            var r = GetRepository(parentId, parentRepositoryName);
            var apiEntity = await r.GetByIdAsync(CancellationToken.None, id);

            return Mapper.MapToDTO(apiEntity);
        }

        public TEntityDto InitializeNew()
        {
            return new TEntityDto
            {

            };
        }

        public async Task<TEntityDto> SaveAsync(int parentId, string parentRepositoryName, TEntityDto data)
        {
            var r = GetRepository(parentId, parentRepositoryName);

            var apiEntity = Mapper.MapToEntity(data);

            if (data.Id == 0)
            {
                await r.InsertAsync(apiEntity);
            }
            else
            {
                await r.UpdateAsync(apiEntity);
            }
            return Mapper.MapToDTO(apiEntity);
        }

        public async Task FillDataSetAsync(GridViewDataSet<TEntityDto> dataSet, IdFilter filter)
        {
            var q = QueryFactory();
            q.Filter = filter;
            await dataSet.LoadFromQueryAsync(q);
        }

        protected abstract IAsyncRepository<TApiEntity, int> GetRepository(int parentId, string parentRepositoryName);
    }
}
