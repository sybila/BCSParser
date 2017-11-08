using AutoMapper;
using Bcs.Admin.Web.ViewModels;
using BcsAdmin.BL.Dto;
using BcsAdmin.DAL.Models;
using DotVVM.Framework.Controls;
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
        private readonly IMapper mapper;

        public DashboardFacade(Func<AppDbContext> dbContextFunc, IMapper mapper)
        {
            this.dbContextFunc = dbContextFunc;
            this.mapper = mapper;
        }

        public BiochemicalEntityDetailDto GetEntityDetail(int id)
        {
            using (var dbContext = dbContextFunc())
            {
                var entity = dbContext.EpEntity
                    .Include(e => e.Locations)
                        .ThenInclude(el => el.Location)
                    .Include(e => e.Components)
                        .ThenInclude(el => el.Component)
                     .Include(e => e.Classifications)
                        .ThenInclude(el => el.Classification)
                    .Include(e => e.Children)
                    .SingleOrDefault(e => e.Id == id);

                return new BiochemicalEntityDetailDto
                {
                    Id = entity.Id,
                    Code = entity.Code,
                    Name = entity.Name,
                    Active = entity.Active == 0,
                    Description = entity.Description,
                    Parent = mapper.Map<BiochemicalEntityLinkDto>(entity.Parent),
                    SelectedHierarchyType = (int)entity.HierarchyType,
                    Components =
                        entity.HierarchyType == HierarchyType.Atomic
                        ? entity.Children.Select(mapper.Map<BiochemicalEntityLinkDto>).ToList()
                        : entity.Components.Select(c => mapper.Map<BiochemicalEntityLinkDto>(c.Component)).ToList(),
                    HierarchyTypes =
                        Enum.GetValues(typeof(HierarchyType))
                        .Cast<HierarchyType>()
                        .Select(v => new BiochemicalEntityTypeDto
                        {
                            Id = (int)v,
                            Name = v.ToString("F")
                        })
                        .ToList(),
                    Locations = entity.Locations.Select(el => mapper.Map<BiochemicalEntityLinkDto>(el.Location)).ToList(),
                    Notes = entity.Notes.Select(mapper.Map<EntityNoteDto>).ToList(),
                    Classifications = entity.Classifications.Select(ec => mapper.Map<ClassificationDto>(ec.Classification)).ToList(),
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
                    Name = $"{entity.HierarchyType}: {entity.Code}"
                };
        }

        public GridViewDataSetLoadedData<BiochemicalEntityRowDto> GetBiochemicalEntityRows(IGridViewDataSetLoadOptions options, string searchText, List<string> entityTypeFilter)
        {
            using (var context = dbContextFunc())
            {
                context.EpEntityLocation.Load();

                var queriable =
                    context.EpEntity
                    .Include(e => e.Locations)
                        .ThenInclude(el => el.Location)
                    .Include(e => e.Components)
                        .ThenInclude(el => el.Component)
                     .Include(e => e.Classifications)
                        .ThenInclude(el => el.Classification)
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
                    });

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    queriable = queriable.Where(e
                        => e.Code.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1
                        || e.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1);
                }
                if (entityTypeFilter.Any())
                {
                    queriable = queriable.Where(e => entityTypeFilter.Any(f => e.Type.Equals(f, StringComparison.OrdinalIgnoreCase)));
                }

                return queriable.GetDataFromQueryable(options);
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
