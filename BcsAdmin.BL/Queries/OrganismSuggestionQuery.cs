using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Dto;

namespace BcsAdmin.BL.Queries
{
    public class OrganismSuggestionQuery : EntityFrameworkQuery<SuggestionDto>, IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        public SuggestionFilter Filter { get; set; }

        public OrganismSuggestionQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<SuggestionDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();

            var queriable =
                    context
                    .EpOrganism
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                queriable = queriable.Where(e
                    => e.Code.IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1
                    || e.Name.IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1);
            }

            return queriable
                .OrderBy(e => e.Code)
                .Take(10)
                .Select(e => new SuggestionDto
                {
                    Id = e.Id,
                    Description = e.Name,
                    Name = e.Code
                });
        }
    }
}
