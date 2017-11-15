using AutoMapper;
using Bcs.Admin.Web.ViewModels;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Dto.Repositories;
using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Mappers;
using BcsAdmin.BL.Queries;
using BcsAdmin.BL.Repositories;
using BcsAdmin.DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.BL
{
    public static class Registry
    {
        public static void RegisterDependenciesBL(this IServiceCollection services)
        {
            services.AddTransient<Func<AppDbContext>>(s => () => new AppDbContext());
            services.AddTransient<AppDbContext, AppDbContext>();

            services.AddTransient<IDateTimeProvider, UtcDateTimeProvider>();
            services.AddTransient<IEntityFrameworkUnitOfWorkProvider<AppDbContext>, EntityFrameworkUnitOfWorkProvider<AppDbContext>>();
            services.AddTransient<IUnitOfWorkProvider, EntityFrameworkUnitOfWorkProvider<AppDbContext>>();
            services.AddSingleton<IUnitOfWorkRegistry, AsyncLocalUnitOfWorkRegistry>();

            services.AddTransient<IRepository<EpEntity, int>, EntityRepisitory>();

            services.AddTransient<EntityClassificationRepository, EntityClassificationRepository>();
            services.AddTransient<EntityNoteRepository, EntityNoteRepository>();
            services.AddTransient<EntityLocationRepository, EntityLocationRepository>();
            services.AddTransient<EntityComponentRepository, EntityComponentRepository>();

            services.RegisterFactory<IFilteredQuery<BiochemicalEntityRowDto, BiochemicalEntityFilter>, EntityGridQuery>();
            services.RegisterFactory<EntityTypeNamesQuery, EntityTypeNamesQuery>();
            services.RegisterFactory<ClassificationQuery, ClassificationQuery>();
            services.RegisterFactory<NoteQuery, NoteQuery>();
            services.RegisterFactory<ComponentLinkQuery, ComponentLinkQuery>();
            services.RegisterFactory<LocationLinkQuery, LocationLinkQuery>();

            services.AddTransient<DashboardFacade, DashboardFacade>();
            services.AddTransient<BasicListFacade, BasicListFacade>();
            services.AddTransient<ClassificationGridFacade, ClassificationGridFacade>();
            services.AddTransient<NoteGridFacade, NoteGridFacade>();
            services.AddTransient<ComponentsGridFacade, ComponentsGridFacade>();
            services.AddTransient<LocationGridFacade, LocationGridFacade>();

            services.RegisterDependantFacade<ClassificationDto, ClassificationGridFacade>();
            services.RegisterDependantFacade<EntityNoteDto, NoteGridFacade>();
            services.RegisterDependantFacade<LocationLinkDto, LocationGridFacade>();
            services.RegisterDependantFacade<ComponentLinkDto, ComponentsGridFacade>();

            services.AddTransient<IEntityDTOMapper<EpEntity, BiochemicalEntityDetailDto>, DetailMapper>();

            services.AddTransient<IEntityDTOMapper<EpEntityClassification, ClassificationDto>, AutoDtoMapper<EpEntityClassification, ClassificationDto>>();
            services.AddTransient<IEntityDTOMapper<EpEntityLocation, LocationLinkDto>, AutoDtoMapper<EpEntityLocation, LocationLinkDto>>();
            services.AddTransient<IEntityDTOMapper<EpEntityComposition, ComponentLinkDto>, AutoDtoMapper<EpEntityComposition, ComponentLinkDto>>();
            services.AddTransient<IEntityDTOMapper<EpEntityNote, EntityNoteDto>, AutoDtoMapper<EpEntityNote, EntityNoteDto>>();
        }

        public static void RegisterMapperBL(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<EpClassification, ClassificationDto>();
            cfg.CreateMap<EpEntityNote, EntityNoteDto>();
            cfg.CreateMap<EpEntity, BiochemicalEntityLinkDto>()
                .ForMember(t => t.HierarchyType, a => a.MapFrom(s => s.HierarchyType));
        }

        public static void RegisterFactory<TContract, TImplementation>(this IServiceCollection services)
            where TContract : class
            where TImplementation : class, TContract
        {
            services.AddTransient<TContract, TImplementation>();
            services.AddTransient<Func<TContract>>(s => () => s.GetRequiredService<TContract>());
        }

        private static void RegisterDependantFacade<TGridEntity, TImpl>(this IServiceCollection services)
            where TGridEntity : IEntity<int>
            where TImpl : class, ICrudFilteredFacade<TGridEntity, TGridEntity, IdFilter, int>
        {
            services.AddTransient<ICrudFilteredFacade<TGridEntity, TGridEntity, IdFilter, int>, TImpl>();
        }
    }
}
