using System;
using BcsAdmin.BL.Repositories.Api;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;

namespace BcsAdmin.BL.Facades
{
    public interface IAsyncCrudFacade<TEntity, TKey, TListDTO, TDetailDTO, TFilter> : IAsyncDetailFacade<TDetailDTO, TKey>, IQueryFacade<TListDTO, TFilter>
        where TEntity : IEntity<TKey>
        where TDetailDTO : IEntity<TKey>
    {
        IEntityDTOMapper<TEntity, TDetailDTO> Mapper { get; }
        IAsyncRepository<TEntity, TKey> Repository { get; }
    }
}