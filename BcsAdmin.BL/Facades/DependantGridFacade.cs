using System;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.BL.Filters;
using BcsAdmin.DAL.Models;
using DotVVM.Framework.Controls;
using System.Threading.Tasks;
using BcsAdmin.BL.Facades.Defs;

namespace BcsAdmin.BL.Facades
{
    public abstract class DependantGridFacade<TApiEntity, TEntityDto> : IGridFacade<int, TEntityDto>
        where TApiEntity : IEntity<int>
        where TEntityDto : class, IEntity<int>, new()
    {
        protected Func<IRepository<TApiEntity, int>> RepositoryFactory { get; }
        protected IEntityDTOMapper<TApiEntity, TEntityDto> Mapper { get; }
        public Func<IFilteredQuery<TEntityDto, IdFilter>> QueryFactory { get; }

        public DependantGridFacade(
            Func<IFilteredQuery<TEntityDto, IdFilter>> queryFactory,
            Func<IRepository<TApiEntity, int>> repositoryFactory,
            IEntityDTOMapper<TApiEntity, TEntityDto> mapper)
        {
            QueryFactory = queryFactory;
            this.RepositoryFactory = repositoryFactory;
            this.Mapper = mapper;
        }

        public void Delete(int parentId, string parentRepositoryName, int id)
        {
            var r = GetRepository(parentId, parentRepositoryName);
            r.Delete(id);
        }

        public TEntityDto GetDetail(int parentId, string parentRepositoryName, int id)
        {
            var r = GetRepository(parentId, parentRepositoryName);
            var apiEntity = r.GetById(id);

            return Mapper.MapToDTO(apiEntity);
        }

        public TEntityDto InitializeNew()
        {
            return new TEntityDto
            {

            };
        }

        public TEntityDto Save(int parentId, string parentRepositoryName, TEntityDto data)
        {
            var r = GetRepository(parentId, parentRepositoryName);

            var apiEntity = Mapper.MapToEntity(data);

            if (data.Id == 0)
            {
                r.Insert(apiEntity);
            }
            else
            {
                r.Update(apiEntity);
            }
            return Mapper.MapToDTO(apiEntity);
        }

        public async Task FillDataSetAsync(GridViewDataSet<TEntityDto> dataSet, IdFilter filter)
        {
            var q = QueryFactory();
            q.Filter = filter;
            await dataSet.LoadFromQueryAsync(q);
        }

        protected abstract IRepository<TApiEntity, int> GetRepository(int parentId, string parentRepositoryName);
    }
}
