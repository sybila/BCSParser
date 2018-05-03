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

namespace BcsAdmin.BL.Facades
{
    public abstract class DependantLinkGridFacade<TParentEntity, TEntity, TEntityDto> : ILinkGridFacade<TEntityDto>
        where TParentEntity : IEntity<int>
        where TEntity : IEntity<int>
        where TEntityDto : IEntity<int>, new()
    {
        public Func<IFilteredQuery<TEntityDto, IdFilter>> QueryFactory { get; set; }

        private readonly IRepository<TEntity, int> associatedEntityRepository;
        private readonly IMapper mapper;

        public DependantLinkGridFacade(
            IRepository<TEntity, int> childEntityRepository,
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

        public void Link(string paentRepositoryName, EntityLinkDto link)
        {
            var parentRepository = GetParentRepository(paentRepositoryName);
            var parentEntity = parentRepository.GetById(link.DetailId);

            LinkCore(parentEntity, link.AssociatedId);
            parentRepository.Update(parentEntity);
        }

        public void CreateAndLink(int parentId, string paentRepositoryName, TEntityDto entity)
        {

            var newEntity = associatedEntityRepository.InitializeNew();
            newEntity.Id = -1;
            mapper.Map(entity, newEntity);
            associatedEntityRepository.Insert(newEntity);

            Link(paentRepositoryName, new EntityLinkDto { DetailId = parentId, AssociatedId = newEntity.Id });
        }

        public void Edit(TEntityDto entityDto)
        {
            var entity = associatedEntityRepository.GetById(entityDto.Id);
            mapper.Map(entityDto, entity);
            associatedEntityRepository.Update(entity);
        }

        public void Unlink(string paentRepositoryName, EntityLinkDto link)
        {
            var parentRepository = GetParentRepository(paentRepositoryName);
            var parentEntity = parentRepository.GetById(link.DetailId);

            UnlinkCore(parentEntity, link.AssociatedId);

            parentRepository.Update(parentEntity);
        }

        protected abstract void UnlinkCore(TParentEntity parentEntity, int associatedId);
        internal abstract void LinkCore(TParentEntity parentEntity, int associatedId);
        protected abstract IRepository<TParentEntity, int> GetParentRepository(string paentRepositoryName);
    }
}
