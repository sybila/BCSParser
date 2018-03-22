using BcsAdmin.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Dto;

namespace BcsAdmin.BL.Queries
{
    public class EntitySuggestionQuery : AppQuery<SuggestionDto>, IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        public SuggestionFilter Filter { get; set; }

        public EntitySuggestionQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<SuggestionDto> GetQueryable()
        {
            var alloedTypes = Filter?.AllowedEntityTypes ?? new Dto.HierarchyType[] { };

            var context = Context.CastTo<AppDbContext>();

            var queriable =
                    context
                    .EpEntity
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Filter?.SearchText))
            {
                queriable = queriable
                    .Where(e=> alloedTypes.Contains((Dto.HierarchyType)e.HierarchyType))
                    .Where(e
                    => (e.Code?? "").IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1
                    || (e.Name?? "").IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1);
            }

            return queriable
                .ToList()
                .AsQueryable()
                .OrderBy(e => e.Code)
                .Take(20)
                .Select(e => new SuggestionDto
                {
                    Id = e.Id,
                    Description = e.Name,
                    Name = e.Code
                });

        }
    }
}
