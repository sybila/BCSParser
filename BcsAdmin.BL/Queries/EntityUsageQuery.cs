using BcsAdmin.BL.Dto;
using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BcsAdmin.BL.Queries
{
    public class EntityUsageQuery : IdFilteredQuery<EntityUsageDto>
    {
        public EntityUsageQuery(IUnitOfWorkProvider unitOfWorkProvider) : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<EntityUsageDto> GetQueryable()
        {
            var parents = Context.Set<EpEntity>()
                .Where(e => e.Children.Any(c => c.Id == Filter.Id))
                .Select(e => new EntityUsageDto
                {
                    Id = e.Id,
                    CategoryType = CategoryType.Entity,
                    FullName = $"{e.Code} - {e.Name}"
                });

            var composites = Context.Set<EpEntityComposition>()
                .Where(e=> e.Component != null && e.ComposedEntity != null)
                .Where(e => e.Component.Id == Filter.Id)
                .Select(e => new EntityUsageDto
                {
                    Id = e.ComposedEntity.Id,
                    CategoryType = CategoryType.Entity,
                    FullName = $"{e.ComposedEntity.Code} - {e.ComposedEntity.Name}"
                });

            var located = Context.Set<EpEntityLocation>()
                .Where(e => e.Entity != null && e.Location != null)
                .Where(e => e.Location.Id == Filter.Id)
                .Select(e => new EntityUsageDto
                {
                    Id = e.Entity.Id,
                    CategoryType = CategoryType.Entity,
                    FullName = $"{e.Entity.Code} - {e.Entity.Name}"
                });

            return parents
                .Concat(composites)
                .Concat(located);
        }
    }
}
