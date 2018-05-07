using BcsAdmin.BL.Repositories.Api;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Facades
{

    public class AsyncCrudFacadeBase<TEntity, TKey, TListDTO, TDetailDTO, TFilter> : IAsyncCrudFacade<TEntity, TKey, TListDTO, TDetailDTO, TFilter>
        where TEntity : IEntity<TKey> where TDetailDTO : IEntity<TKey>
    {
        public Func<IFilteredQuery<TListDTO, TFilter>> QueryFactory { get; }
        public IAsyncRepository<TEntity, TKey> Repository { get; }
        public IEntityDTOMapper<TEntity, TDetailDTO> Mapper { get; }

        protected AsyncCrudFacadeBase(Func<IFilteredQuery<TListDTO, TFilter>> queryFactory, IAsyncRepository<TEntity, TKey> repository, IEntityDTOMapper<TEntity, TDetailDTO> mapper)
        {
            QueryFactory = queryFactory;
            Repository = repository;
            Mapper = mapper;
        }

        public virtual async Task<TDetailDTO> GetDetailAsync(TKey id)
        {
            var entity = await Repository.GetByIdAsync(CancellationToken.None, id);
            ValidateReadPermissions(entity);
            var detail = Mapper.MapToDTO(entity);
            return detail;
        }

        public virtual TDetailDTO InitializeNew()
        {
            var entity = Repository.InitializeNew();
            var detail = Mapper.MapToDTO(entity);
            return detail;
        }

        public virtual async Task<TDetailDTO> SaveAsync(TDetailDTO detail)
        {

            TEntity entity;
            var isNew = false;
            if (detail.Id.Equals(default(TKey)))
            {
                // the record is new
                entity = Repository.InitializeNew();
                isNew = true;
            }
            else
            {
                entity = await Repository.GetByIdAsync(CancellationToken.None, detail.Id);
                ValidateModifyPermissions(entity, ModificationStage.BeforeMap);
            }

            // populate the entity
            PopulateDetailToEntity(detail, entity);

            ValidateModifyPermissions(entity, ModificationStage.AfterMap);

            // save
            return await SaveAsync(entity, isNew, detail);
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            await Repository.DeteleAsync(id);
        }

        protected virtual void PopulateDetailToEntity(TDetailDTO detail, TEntity entity)
        {
            Mapper.PopulateEntity(detail, entity);
        }

        protected virtual async Task<TDetailDTO> SaveAsync(TEntity entity, bool isNew, TDetailDTO detail)
        {
            // insert or update
            if (isNew)
            {
                await Repository.InsertAsync(entity);
            }
            else
            {
                await Repository.UpdateAsync(entity);
            }

            // save
            detail.Id = entity.Id;
            var savedDetail = Mapper.MapToDTO(entity);
            return savedDetail;
        }

        protected virtual void ValidateReadPermissions(TEntity entity)
        {
        }

        protected virtual void ValidateModifyPermissions(TEntity entity, ModificationStage stage)
        {
        }
    }
}
