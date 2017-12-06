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

namespace BcsAdmin.BL
{
    public static class Registry
    {
        public static void RegisterMapperBL(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<EpReaction, ReactionRowDto>();

            cfg.CreateMap<EpClassification, ClassificationDto>();
            cfg.CreateMap<EpEntityNote, EntityNoteDto>();
            cfg.CreateMap<EpEntity, ComponentLinkDto>()
                .ForMember(t => t.HierarchyType, a => a.MapFrom(s => s.HierarchyType));
            cfg.CreateMap<ComponentLinkDto, EpEntity>()
                .ForMember(t => t.Code, m => m.MapFrom(s => s.Code))
                .ForMember(t => t.Name, m => m.MapFrom(s => s.Name))
                .ForMember(t => t.HierarchyType, m => m.MapFrom(s => s.HierarchyType))
                .ForMember(t => t.Id, m => m.MapFrom(s => s.Id))
                .ForAllOtherMembers(a => a.Ignore());
            cfg.CreateMap<EpOrganism, EntityOrganismDto>()
                .ForMember(m => m.GeneGroup, a => a.Ignore())
                .ForMember(m=> m.IntermediateEntityId, a=> a.Ignore());

            cfg.CreateMap<EpEntity, LocationLinkDto>()
                .ForMember(t => t.HierarchyType, a => a.MapFrom(s => s.HierarchyType));
            cfg.CreateMap<LocationLinkDto, EpEntity>()
                .ForMember(t => t.Code, m => m.MapFrom(s => s.Code))
                .ForMember(t => t.Name, m => m.MapFrom(s => s.Name))
                .ForMember(t => t.HierarchyType, m => m.MapFrom(s => s.HierarchyType))
                .ForMember(t => t.Id, m => m.MapFrom(s => s.Id))
                .ForAllOtherMembers(a => a.Ignore());

            cfg.CreateMap<EpEntity, LocationLinkDto>()
               .ForMember(t => t.HierarchyType, a => a.MapFrom(s => s.HierarchyType));
            cfg.CreateMap<EntityLinkDto, EpEntityClassification>()
                .ForMember(s => s.ClassificationId, a => a.MapFrom(s => s.AssociatedId))
                .ForMember(s => s.EntityId, a => a.MapFrom(s => s.DetailId));
            cfg.CreateMap<EntityLinkDto, EpEntityComposition>()
               .ForMember(s => s.ChildEntityId, a => a.MapFrom(s => s.AssociatedId))
               .ForMember(s => s.ParentEntityId, a => a.MapFrom(s => s.DetailId));
            cfg.CreateMap<EntityLinkDto, EpEntityLocation>()
                .ForMember(s => s.ParentEntityId, a => a.MapFrom(s => s.AssociatedId))
                .ForMember(s => s.ChildEntityId, a => a.MapFrom(s => s.DetailId));
            cfg.CreateMap<EntityLinkDto, EpEntityOrganism>()
                .ForMember(s=> s.EntityId, a=> a.MapFrom(s=> s.DetailId))
                .ForMember(s => s.OrganismId, a => a.MapFrom(s => s.AssociatedId));

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
