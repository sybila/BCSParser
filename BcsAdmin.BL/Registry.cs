using AutoMapper;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Dto.Repositories;
using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Mappers;
using BcsAdmin.BL.Queries;
using BcsAdmin.BL.Repositories;
using BcsAdmin.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using Bcs.Admin.BL.Dto;
using BcsAdmin.DAL.Api;

namespace BcsAdmin.BL
{
    public static class Registry
    {
        public static void RegisterMapperBL(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ApiRule, ReactionRowDto>();
            cfg.CreateMap<ApiRule, BiochemicalReactionDetailDto>();
            cfg.CreateMap<BiochemicalReactionDetailDto, ApiRule> ();

            cfg.CreateMap<ApiClassification, ClassificationDto>();
            cfg.CreateMap<EpEntityNote, EntityNoteDto>();

            //Entity mapped grid entities
            cfg.CreateMap<ApiEntity, ComponentLinkDto>();
            cfg.CreateMap<ComponentLinkDto, ApiEntity>()
                .ForMember(t => t.Code, m => m.MapFrom(s => s.Code))
                .ForMember(t => t.Name, m => m.MapFrom(s => s.Name))
                .ForMember(t => t.Type, m => m.MapFrom(s => s.HierarchyType))
                .ForMember(t => t.Id, m => m.MapFrom(s => s.Id))
                .ForAllOtherMembers(a => a.Ignore());
            cfg.CreateMap<ApiEntity, LocationLinkDto>()
                .ForMember(m => m.IntermediateEntityId, a => a.Ignore());
            cfg.CreateMap<LocationLinkDto, ApiEntity>()
                .ForMember(t => t.Code, m => m.MapFrom(s => s.Code))
                .ForMember(t => t.Name, m => m.MapFrom(s => s.Name))
                .ForMember(t => t.Type, m => m.MapFrom(s => (int)Dto.HierarchyType.Compartment))
                .ForMember(t => t.Id, m => m.MapFrom(s => s.Id))
                .ForAllOtherMembers(a => a.Ignore());
            cfg.CreateMap<ApiEntity, StateEntityDto>();
            cfg.CreateMap<StateEntityDto, ApiEntity>()
               .ForMember(t => t.Code, m => m.MapFrom(s => s.Code))
                .ForMember(t => t.Type, m => m.MapFrom(s => (int)Dto.HierarchyType.State))
                .ForMember(t => t.Id, m => m.MapFrom(s => s.Id))
                .ForAllOtherMembers(a => a.Ignore());

            //other grid entities
            cfg.CreateMap<ApiOrganism, EntityOrganismDto>()
                .ForMember(m => m.GeneGroup, a => a.Ignore())
                .ForMember(m => m.Code, a => a.Ignore())
                .ForMember(m => m.IntermediateEntityId, a => a.Ignore());

            //Entity notes
            cfg.CreateMap<EpEntityNote, EntityNoteDto>()
                .ForMember(m => m.IntermediateEntityId, a => a.MapFrom(s => s.EntityId));
            cfg.CreateMap<EntityNoteDto, EpEntityNote>()
                .ForMember(m => m.EntityId, a => a.MapFrom(s => s.IntermediateEntityId))
                .ForMember(m => m.Inserted, a => a.Ignore())
                .ForMember(m => m.Updated, a => a.Ignore())
                .ForMember(m => m.User, a => a.Ignore())
                .ForMember(m => m.UserId, a => a.Ignore());
        }

        public static void RegisterFactory<TContract, TImplementation>(this IServiceCollection services)
            where TContract : class
        {
            services.AddTransient<Func<TContract>>(s => () => s.GetRequiredService<TContract>());
        }
    }
}
