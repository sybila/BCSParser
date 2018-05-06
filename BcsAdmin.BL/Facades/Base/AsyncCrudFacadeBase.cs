using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Facades
{

    public class AsyncCrudFacadeBase<TEntity, TKey, TListDTO, TDetailDTO, TFilter>
        where TEntity : IEntity<TKey> where TDetailDTO : IEntity<TKey>
    {
        public Func<IFilteredQuery<TListDTO, TFilter>> QueryFactory { get; }
        public IRepository<TEntity, TKey> Repository { get; }
        public IEntityDTOMapper<TEntity, TDetailDTO> Mapper { get; }

        protected AsyncCrudFacadeBase(Func<IFilteredQuery<TListDTO, TFilter>> queryFactory, IRepository<TEntity, TKey> repository, IEntityDTOMapper<TEntity, TDetailDTO> mapper)
        {
            QueryFactory = queryFactory;
            Repository = repository;
            Mapper = mapper;
        }

        public virtual TDetailDTO GetDetail(TKey id)
        {
            var entity = Repository.GetById(id);
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

        public virtual TDetailDTO Save(TDetailDTO detail)
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
                entity = Repository.GetById(detail.Id);
                ValidateModifyPermissions(entity, ModificationStage.BeforeMap);
            }

            // populate the entity
            PopulateDetailToEntity(detail, entity);

            ValidateModifyPermissions(entity, ModificationStage.AfterMap);

            // save
            return Save(entity, isNew, detail);
        }

        public virtual void Delete(TKey id)
        {
            Repository.Delete(id);

        }

        public virtual IEnumerable<TListDTO> GetList(TFilter filter, Action<IFilteredQuery<TListDTO, TFilter>> queryConfiguration = null)
        {

            var query = QueryFactory();
            queryConfiguration?.Invoke(query);
            return query.Execute();

        }

        protected virtual void PopulateDetailToEntity(TDetailDTO detail, TEntity entity)
        {
            Mapper.PopulateEntity(detail, entity);
        }

        protected virtual TDetailDTO Save(TEntity entity, bool isNew, TDetailDTO detail)
        {
            // insert or update
            if (isNew)
            {
                Repository.Insert(entity);
            }
            else
            {
                Repository.Update(entity);
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
