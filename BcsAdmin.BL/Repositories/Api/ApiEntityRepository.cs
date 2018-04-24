using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Repositories.Api
{

    namespace BcsAdmin.BL.Repositories
    {
        public class ApiEntityRepository : ApiGenericRepository<ApiEntity>
        {
            public ApiEntityRepository(IDateTimeProvider dateTimeProvider)
                : base(dateTimeProvider)
            {
                RepoName = "entities";
            }

            public override ApiEntity InitializeNew()
            {
                var @new = base.InitializeNew();
                @new.Type = ApiEntityType.Atomic;
                @new.Status = ApiEntityStatus.Inactive;
                return @new;
            }

            public override Task<IList<ApiEntity>> GetByIdsAsync(CancellationToken cancellationToken, IEnumerable<int> ids, params Expression<Func<ApiEntity, object>>[] includes)
            {
                return base.GetByIdsAsync(cancellationToken, ids, includes);
            }
        }
    }

}
