using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using BcsAdmin.BL.Filters;
using Bcs.Admin.BL.Dto;
using System.Threading;
using System.Threading.Tasks;
using BcsAdmin.BL.Repositories.Api;

namespace BcsAdmin.BL.Queries
{
    public class EntityGridQuery : AppApiQuery<BiochemicalEntityRowDto>, IFilteredQuery<BiochemicalEntityRowDto, BiochemicalEntityFilter>
    {

        public BiochemicalEntityFilter Filter { get; set; }

        private IAsyncRepository<ApiEntity, int> entityRepository;

        public EntityGridQuery(IAsyncRepository<ApiEntity, int> repository)
        {
            this.entityRepository = repository;
        }

        protected async override Task<IQueryable<BiochemicalEntityRowDto>> GetQueriableAsync(CancellationToken cancellationToken)
        {
            var data = await GetWebDataAsync<ApiQueryEntity>(cancellationToken, "entities");

            var queriable = data.Select(e => new BiochemicalEntityRowDto
            {
                Id = e.Id,
                Name = e.Name,
                Code = e.Code,              
                Type = e.Type != null ? e.Type.Value.ToString("F") : "<null>",          
                EntityTypeCss = $"entity-{(e.Type != null ? e.Type.Value.ToString("F") : "unknown")}".ToLower()
            });

            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                queriable = queriable.Where(e
                     => (e.Code != null ? e.Code : "").IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1
                     || (e.Name != null ? e.Name : "").IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1);
            }
            if (Filter.EntityTypeFilter.Any())
            {
                queriable = queriable.Where(e => Filter.EntityTypeFilter.Any(f => f.Equals(e.Type, StringComparison.OrdinalIgnoreCase)));
            }
            return queriable;
        }
    }
}
