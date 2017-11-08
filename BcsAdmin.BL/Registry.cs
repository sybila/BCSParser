using AutoMapper;
using BcsAdmin.BL.Dto;
using BcsAdmin.DAL.Models;
using Microsoft.Extensions.DependencyInjection;
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
        }

        public static void RegisterMapperBL(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<EpClassification, ClassificationDto>();
            cfg.CreateMap<EpEntityNote, EntityNoteDto>();
            cfg.CreateMap<EpEntity, BiochemicalEntityLinkDto>()
                .ForMember(t => t.HierarchyType, a => a.MapFrom(s=> s.HierarchyType));
        }
    }
}
