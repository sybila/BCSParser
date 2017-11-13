using BcsAdmin.BL.Queries;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.BL.Facades
{
    public class BasicListFacade : FacadeBase
    {
        private readonly Func<EntityTypeQuery> entityTypeQueryFunc;

        public BasicListFacade(IUnitOfWorkProvider unitOfWorkProvider, Func<EntityTypeQuery> entityTypeQueryFunc)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
            this.entityTypeQueryFunc = entityTypeQueryFunc;
        }

        public IList<string> GetEntityTypes()
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var q = entityTypeQueryFunc();
                return q.Execute();
            }
        }
    }
}
