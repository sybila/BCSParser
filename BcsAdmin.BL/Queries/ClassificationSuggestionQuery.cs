using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Queries
{
    public class ClassificationSuggestionQuery : AppApiQuery<SuggestionDto>, IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        public SuggestionFilter Filter { get; set; }

        protected override async Task<IQueryable<SuggestionDto>> GetQueriableAsync(CancellationToken cancellationToken)
        {
            var queriable = await GetWebDataAsync<ApiClassification>(cancellationToken, "classifications");

            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                queriable = queriable
                    .Where(e => e.Name != null)
                    .Where(e => e.Name.IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1)
                    .Where(e => Filter.Category.TypeEquals(e.Type));
            }

            return queriable
                .OrderBy(e => e.Name)
                .Take(10)
                .Select(e => new SuggestionDto
                {
                    Id = e.Id,
                    Description = e.Type.ToString(),
                    Name = e.Name
                });
        }
    }
}
