using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System.Linq.Expressions;
using BcsAdmin.BL.Repositories.Api;

namespace BcsAdmin.BL.Repositories
{

    public class AppGenericRepository<TEntity> : EntityFrameworkRepository<TEntity, int>, IRepository<TEntity, int>
        where TEntity : class, IEntity<int>, new()
    {
        public AppGenericRepository(IUnitOfWorkProvider unitOfWorkProvider, IDateTimeProvider dateTimeProvider)
            : base(unitOfWorkProvider, dateTimeProvider)
        {
        }
    }
}
