using BcsAdmin.DAL.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Queries
{
    public class EntitySuggestionQuery : AppApiQuery<SuggestionDto>, IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        public SuggestionFilter Filter { get; set; }

        public EntitySuggestionQuery()
        {
            RepoName = "entities";
        }

        protected async override Task<IQueryable<SuggestionDto>> GetQueriableAsync(CancellationToken cancellationToken)
        {
            var alloedTypes = Filter?.AllowedEntityTypes ?? new Dto.HierarchyType[] { };

            if (string.IsNullOrWhiteSpace(Filter?.SearchText))
            {
                return Enumerable.Empty<SuggestionDto>().AsQueryable();
            }

            var data = await GetWebDataAsync<ApiEntity>(cancellationToken,"entities");

            var entities = data.Where(e => alloedTypes.Contains((Dto.HierarchyType)e.Type));


            var codeStartsWith = entities
                .Where(e => (e.Code ?? "").StartsWith(Filter.SearchText, StringComparison.OrdinalIgnoreCase))
                .OrderBy(e => e.Code);

            var nameStartsWith = entities
               .Where(e => (e.Name ?? "").StartsWith(Filter.SearchText, StringComparison.OrdinalIgnoreCase))
               .OrderBy(e => e.Name);


            var a = codeStartsWith
                .Concat(nameStartsWith)
                .Distinct()
                .Take(20)
                .ToList();

            return
                a.Select(e => new SuggestionDto
                {
                    Id = e.Id,
                    Description = e.Name,
                    Name = e.Code
                }).AsQueryable();
        }
    }
}
