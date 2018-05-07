using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Repositories.Api
{
    public interface IAsyncRepository<TEntity, TKey> 
        where TEntity : IEntity<TKey>
    {
        string RepoName { get; set; }

        Task DeteleAsync(TKey id);
        Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, TKey id);
        Task<IList<TEntity>> GetByIdsAsync(CancellationToken cancellationToken, IEnumerable<TKey> ids);
        TEntity InitializeNew();
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
    }
}