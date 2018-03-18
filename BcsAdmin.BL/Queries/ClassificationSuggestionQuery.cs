using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Dto;

namespace BcsAdmin.BL.Queries
{
    public class ClassificationSuggestionQuery : EntityFrameworkQuery<SuggestionDto>, IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        public SuggestionFilter Filter { get; set; }

        public ClassificationSuggestionQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<SuggestionDto> GetQueryable()
        {

            var context = Context.CastTo<AppDbContext>();

            var queriable =
                    context
                    .EpClassification
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                queriable = queriable
                    .Where(e => e.Name != null)
                    .Where(e => e.Name.IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1);
            }

            return queriable
                .OrderBy(e => e.Name)
                .Take(10)
                .Select(e => new SuggestionDto
                {
                    Id = e.Id,
                    Description = e.Type,
                    Name = e.Name
                });
        }
    }
}
