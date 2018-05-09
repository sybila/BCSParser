using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Repositories.Api
{
    public interface IAsyncReadonlyRepository<TEntity, TKey>
    {
        Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, TKey id);
    }
}