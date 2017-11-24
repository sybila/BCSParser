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
using AutoMapper;
using DotVVM.Framework.Controls;
using Riganti.Utils.Infrastructure;

namespace BcsAdmin.BL.Facades
{
    public class DependantGridFacade<TIntermediateEntity, TEntity, TEntityDto> : FacadeBase, IGridFacade<TEntityDto> 
        where TIntermediateEntity : IEntity<int>
        where TEntity : IEntity<int>
        where TEntityDto : IEntity<int>
    {
        public Func<IFilteredQuery<TEntityDto, IdFilter>> QueryFactory { get; set; }

        private readonly IRepository<TEntity, int> associatedEntityRepository;
        private readonly IRepository<TIntermediateEntity, int> intermediateRepository;
        private readonly IMapper mapper;

        public DependantGridFacade(
            IRepository<TIntermediateEntity, int> intermediateRepository,
            IRepository<TEntity, int> associatedEntityRepository,
            Func<IdFilteredQuery<TEntityDto>> queryFactory,
            IUnitOfWorkProvider unitOfWorkProvider,
            IMapper mapper)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
            this.associatedEntityRepository = associatedEntityRepository;
            this.intermediateRepository = intermediateRepository;
            this.QueryFactory = queryFactory;
            this.mapper = mapper;
        }

        public void FillDataSet(GridViewDataSet<TEntityDto> dataSet, IdFilter filter) 
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = QueryFactory();
                query.Filter = filter;
                dataSet.LoadFromQuery(query);
            }
        }

        public IEnumerable<TEntityDto> GetList(IdFilter filter, Action<IFilteredQuery<TEntityDto, IdFilter>> queryConfiguration = null)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = QueryFactory();
                query.Filter = filter;
                queryConfiguration?.Invoke(query);
                return query.Execute();
            }
        }

        public TEntityDto CreateAssociated()
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var init = associatedEntityRepository.InitializeNew();
                return mapper.Map<TEntityDto>(init);
            }
        }

        public void Link(EntityLinkDto link)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var intermediate = intermediateRepository.InitializeNew();
                mapper.Map(link, intermediate);
                intermediateRepository.Insert(intermediate);
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

        public void Unlink(int intermediateId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                intermediateRepository.Delete(intermediateId);
                uow.Commit();
            }
        }
    }

    public class LocationGridFacade : DependantGridFacade<EpEntityLocation, EpEntity, LocationLinkDto>
    {
        public LocationGridFacade(IRepository<EpEntityLocation, int> intermediateRepository, IRepository<EpEntity, int> associatedEntityRepository, Func<IdFilteredQuery<LocationLinkDto>> queryFactory, IUnitOfWorkProvider unitOfWorkProvider, IMapper mapper) : base(intermediateRepository, associatedEntityRepository, queryFactory, unitOfWorkProvider, mapper)
        {
        }
    }

    public class ComponentsGridFacade : DependantGridFacade<EpEntityComposition, EpEntity, ComponentLinkDto>
    {
        public ComponentsGridFacade(IRepository<EpEntityComposition, int> intermediateRepository, IRepository<EpEntity, int> associatedEntityRepository, Func<IdFilteredQuery<ComponentLinkDto>> queryFactory, IUnitOfWorkProvider unitOfWorkProvider, IMapper mapper) 
            : base(intermediateRepository, associatedEntityRepository, queryFactory, unitOfWorkProvider, mapper)
        {
        }
    }

    public class ClassificationGridFacade : DependantGridFacade<EpEntityClassification, EpClassification, ClassificationDto>
    {
        public ClassificationGridFacade(
            IRepository<EpEntityClassification, int> intermediateRepository, 
            IRepository<EpClassification, int> associatedEntityRepository,
            Func<IdFilteredQuery<ClassificationDto>> queryFactory, 
            IUnitOfWorkProvider unitOfWorkProvider, IMapper mapper)
            : base(intermediateRepository, associatedEntityRepository, queryFactory, unitOfWorkProvider, mapper)
        {
        }
    }

    public class NoteGridFacade : FilteredCrudFacadeBase<EpEntityNote, int, EntityNoteDto, EntityNoteDto, IdFilter>
    {
        protected NoteGridFacade(
            Func<IFilteredQuery<EntityNoteDto, IdFilter>> queryFactory, 
            IRepository<EpEntityNote, int> repository, 
            IEntityDTOMapper<EpEntityNote, EntityNoteDto> mapper) 
            : base(queryFactory, repository, mapper)
        {
        }
    }
}
