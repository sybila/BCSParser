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

            if (string.IsNullOrWhiteSpace(Filter?.SearchText))
            {
                return Enumerable.Empty<SuggestionDto>().AsQueryable();
            }

            var entities = context.EpEntity.Where(e => alloedTypes.Contains((Dto.HierarchyType)e.HierarchyType));


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
