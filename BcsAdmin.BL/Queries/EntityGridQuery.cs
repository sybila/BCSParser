using Bcs.Admin.Web.ViewModels;
using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using BcsAdmin.BL.Filters;

namespace BcsAdmin.BL.Queries
{
    public class EntityGridQuery : EntityFrameworkQuery<BiochemicalEntityRowDto, AppDbContext>, IFilteredQuery<BiochemicalEntityRowDto, BiochemicalEntityFilter>
    {
        public BiochemicalEntityFilter Filter { get; set; }

        public EntityGridQuery(IUnitOfWorkProvider unitOfWorkProvider) 
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<BiochemicalEntityRowDto> GetQueryable()
        {
            Context.EpEntity.Load();
            Context.EpEntityLocation.Load();
            Context.EpEntityComposition.Load();

            var queriable =
                    Context
                    .EpEntity
                    .Select(e => new BiochemicalEntityRowDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Code = e.Code,
                        Locations = e.Locations.Select(el => el.Location.Code).ToList(),
                        Type = e.HierarchyType.ToString("F"),
                        Children = e.HierarchyType == HierarchyType.Atomic
                            ? e.Children.Select(ce => $"{ce.HierarchyType.ToString("F")}: {ce.Code}").ToList()
                            : e.Components.Select(ce => $"{ce.Component.HierarchyType.ToString("F")}: {ce.Component.Code}").ToList(),
                        EntityTypeCss = $"entity-{e.HierarchyType.ToString("F")}".ToLower()
                    });

                if (!string.IsNullOrWhiteSpace(Filter.SearchText))
                {
                    queriable = queriable.Where(e
                        => e.Code.IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1
                        || e.Name.IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1);
                }
                if (Filter.EntityTypeFilter.Any())
                {
                    queriable = queriable.Where(e => Filter.EntityTypeFilter.Any(f => e.Type.Equals(f, StringComparison.OrdinalIgnoreCase)));
                }

            return queriable;
        }
    }
}
