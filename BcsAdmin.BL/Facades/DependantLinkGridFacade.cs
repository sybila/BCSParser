using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Queries;
using BcsAdmin.BL.Repositories;
using AutoMapper;
using DotVVM.Framework.Controls;
using Riganti.Utils.Infrastructure;
using System.Threading.Tasks;
using BcsAdmin.BL.Repositories.Api;
using System.Threading;

namespace BcsAdmin.BL.Facades
{
    public abstract class DependantLinkGridFacade<TParentEntity, TEntity, TEntityDto> : ILinkGridFacade<TEntityDto>
        where TParentEntity : IEntity<int>
        where TEntity : IEntity<int>, new()
        where TEntityDto : IEntity<int>, new()
    {
        public Func<IFilteredQuery<TEntityDto, IdFilter>> QueryFactory { get; set; }

        private readonly IAsyncRepository<TEntity, int> associatedEntityRepository;
        private readonly IMapper mapper;

        public DependantLinkGridFacade(
            IAsyncRepository<TEntity, int> childEntityRepository,
            Func<ManyToManyQuery<TParentEntity, TEntity, TEntityDto>> queryFactory,
            IMapper mapper)
        {
            this.associatedEntityRepository = childEntityRepository;
            this.QueryFactory = queryFactory;
            this.mapper = mapper;
        }

        public async Task FillDataSetAsync(GridViewDataSet<TEntityDto> dataSet, IdFilter filter)
        {
            var query = QueryFactory();
            query.Filter = filter;
            await dataSet.LoadFromQueryAsync(query);
        }

        public TEntityDto CreateAssociated()
        {
            return new TEntityDto();
        }

        public async Task LinkAsync(string paentRepositoryName, EntityLinkDto link)
        {
            var parentRepository = GetParentRepository(paentRepositoryName);
            var parentEntity = await parentRepository.GetByIdAsync(CancellationToken.None, link.DetailId);

            LinkCore(parentEntity, link.AssociatedId);
            await parentRepository.UpdateAsync(parentEntity);
        }

        public async Task CreateAndLinkAsync(int parentId, string paentRepositoryName, TEntityDto entity)
        {

            var newEntity = associatedEntityRepository.InitializeNew();
            newEntity.Id = -1;
            mapper.Map(entity, newEntity);
            await associatedEntityRepository.InsertAsync(newEntity);

            await LinkAsync(paentRepositoryName, new EntityLinkDto { DetailId = parentId, AssociatedId = newEntity.Id });
        }

        public async Task EditAsync(TEntityDto entityDto)
        {
            var entity = await associatedEntityRepository.GetByIdAsync(CancellationToken.None, entityDto.Id);
            mapper.Map(entityDto, entity);
            await associatedEntityRepository.UpdateAsync(entity);
        }

        public async Task UnlinkAsync(string paentRepositoryName, EntityLinkDto link)
        {
            var parentRepository = GetParentRepository(paentRepositoryName);
            var parentEntity = await parentRepository.GetByIdAsync(CancellationToken.None, link.DetailId);

            UnlinkCore(parentEntity, link.AssociatedId);

            await parentRepository.UpdateAsync(parentEntity);
        }

        protected abstract void UnlinkCore(TParentEntity parentEntity, int associatedId);
        internal abstract void LinkCore(TParentEntity parentEntity, int associatedId);
        protected abstract IAsyncRepository<TParentEntity, int> GetParentRepository(string paentRepositoryName);
    }
}
