using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BcsAdmin.BL.Facades
{
    public class UsageFacade : FacadeBase
    {
        private readonly Func<IdFilteredQuery<EntityUsageDto>> entityUsagesQuery;

        public UsageFacade(IUnitOfWorkProvider unitOfWorkProvider, Func<IdFilteredQuery<EntityUsageDto>> entityUsagesQuery)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
            this.entityUsagesQuery = entityUsagesQuery;
        }

        public List<string> GetEntityUsageList(int entityId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var query = entityUsagesQuery();
                query.Filter = new Filters.IdFilter { Id = entityId };
                var usagesList = query.Execute();
                return usagesList.Select(u => $"{u.CategoryType}: {u.FullName}").ToList();
            }
        }
    }
}
