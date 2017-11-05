using Bcs.Admin.Web.ViewModels;
using BcsAdmin.BL.Dto;
using BcsAdmin.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BcsAdmin.BL.Facades
{
    public class DashboardFacade
    {
        private readonly Func<AppDbContext> dbContextFunc;

        public DashboardFacade(Func<AppDbContext> dbContextFunc)
        {
            this.dbContextFunc = dbContextFunc;
        }

        public BiochemicalEntityDetailDto GetEntityDetail(int id)
        {
            using (var dbContext = dbContextFunc())
            {
                var entity = dbContext.EpEntity.Find(id);
                return new BiochemicalEntityDetailDto
                {
                    Id = entity.Id,
                    Code = entity.Code,
                    Name = entity.Name,
                    Active = entity.Active == 0,
                    Description = entity.Description,
                    Parent = ToLink(entity.Parent),
                    Components =
                        entity.HierarchyType == HierarchyType.Atomic
                        ? entity.Children.Select(ToLink).ToList()
                        : entity.Components.Select(c => ToLink(c.Component)).ToList(),
                    HierarchyTypes =
                        Enum.GetValues(typeof(HierarchyType))
                        .Cast<HierarchyType>()
                        .Select(v => new BiochemicalEntityTypeDto
                        {
                            Id = (int)v,
                            Name = v.ToString("F")
                        })
                        .ToList(),
                    Locations = entity.Locations.Select(el => ToLink(el.Location)).ToList(),
                    VisualisationXml = entity.VisualisationXml
                };
            }
        }

        private static BiochemicalEntityLinkDto ToLink(EpEntity entity)
        {
            return entity == null
                ? null
                : new BiochemicalEntityLinkDto
                {
                    Id = entity.Id,
                    Text = $"{entity.HierarchyType}: {entity.Code}"
                };
        }

        public IQueryable<BiochemicalEntityRowDto> GetBiochemicalEntityRows()
        {
            using (var context = dbContextFunc())
            {
                return context.EpEntity
                    .Include(e => e.Components)
                    .Include(e => e.Classifications)
                    .Include(e => e.Locations)
                    .Include(e => e.Children)
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
                    })
                    .ToList()
                    .AsQueryable();
            }
        }

        public List<string> GetEntityTypes()
        {
            using (var context = dbContextFunc())
            {
                return context.EpEntity
                    .Select(e => e.HierarchyType.ToString("F"))
                    .Distinct().ToList();
            }
        }
    }
}
