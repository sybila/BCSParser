using AutoMapper;
using Bcs.Admin.Web.ViewModels;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Dto.Repositories;
using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Mappers;
using BcsAdmin.BL.Queries;
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

            services.RegisterFactory<IFilteredQuery<BiochemicalEntityRowDto, BiochemicalEntityFilter>, EntityGridQuery>();
            services.RegisterFactory<EntityTypeQuery, EntityTypeQuery>();

            services.AddTransient<Func<EntityTypeQuery>>(s => () => s.GetRequiredService<EntityTypeQuery>());

            services.AddTransient<DashboardFacade, DashboardFacade>();
            services.AddTransient<BasicListFacade, BasicListFacade>();


            services.AddTransient<IEntityDTOMapper<EpEntity, BiochemicalEntityDetailDto>, DetailMapper>();
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
    }
}
