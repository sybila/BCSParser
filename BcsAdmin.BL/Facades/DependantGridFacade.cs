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
    public abstract class DependantLinkGridFacade<TParentEntity, TEntity, TEntityDto> : FacadeBase, ILinkGridFacade<TEntityDto>
        where TParentEntity : IEntity<int>
        where TEntity : IEntity<int>
        where TEntityDto : IEntity<int>, new()
    {
        public Func<IFilteredQuery<TEntityDto, IdFilter>> QueryFactory { get; set; }

        private readonly IRepository<TEntity, int> associatedEntityRepository;
        private readonly IRepository<TParentEntity, int> parentRepository;
        private readonly IMapper mapper;

        public DependantLinkGridFacade(
            IRepository<TParentEntity, int> parentRepository,
            IRepository<TEntity, int> associatedEntityRepository,
            Func<ManyToManyQuery<TParentEntity, TEntity, TEntityDto>> queryFactory,
            IUnitOfWorkProvider unitOfWorkProvider,
            IMapper mapper)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
            this.associatedEntityRepository = associatedEntityRepository;
            this.parentRepository = parentRepository;
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

        public void Link(EntityLinkDto link)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var parentEntity = parentRepository.GetById(link.DetailId);

                LinkCore(parentEntity, link.AssociatedId);

                parentRepository.Update(parentEntity);

                uow.Commit();
            }
        }

        public void CreateAndLink(TEntityDto entity, int detailId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var newEntity = associatedEntityRepository.InitializeNew();
                newEntity.Id = -1;
                mapper.Map(entity, newEntity);
                associatedEntityRepository.Insert(newEntity);
                uow.Commit();

                Link(new EntityLinkDto { DetailId = detailId, AssociatedId = newEntity.Id });
                uow.Commit();
            }
        }

        public void Edit(TEntityDto entityDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var entity = associatedEntityRepository.GetById(entityDto.Id);
                mapper.Map(entityDto, entity);
                associatedEntityRepository.Update(entity);
                uow.Commit();
            }
        }

        public void Unlink(EntityLinkDto link)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var parentEntity = parentRepository.GetById(link.DetailId);

                UnlinkCore(parentEntity, link.AssociatedId);

                parentRepository.Update(parentEntity);

                uow.Commit();
            }
        }

        protected abstract void UnlinkCore(TParentEntity parentEntity, int associatedId);
        internal abstract void LinkCore(TParentEntity parentEntity, int associatedId);
    }
}
