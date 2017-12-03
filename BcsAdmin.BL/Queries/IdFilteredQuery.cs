using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using Microsoft.EntityFrameworkCore;

namespace BcsAdmin.BL.Queries
{
    public abstract class IdFilteredQuery<TEntityDto> : EntityFrameworkQuery<TEntityDto>, IFilteredQuery<TEntityDto, IdFilter>
    {
        public IdFilteredQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        public IdFilter Filter { get; set; }
    }

    public class LocationLinkQuery : IdFilteredQuery<LocationLinkDto>
    {
        public LocationLinkQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<LocationLinkDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();
            context.EpEntity.Load();
            return context.EpEntityLocation.Where(e => e.Entity.Id == Filter.Id).Select(e => new LocationLinkDto
            {
                Id = e.Location.Id,
                Code = e.Location.Code,
                HierarchyType = (int)e.Location.HierarchyType,
                Name = e.Location.Name
            });
        }
    }

    public class ComponentLinkQuery : IdFilteredQuery<ComponentLinkDto>
    {
        public ComponentLinkQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<ComponentLinkDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();
            context.EpEntity.Load();
            context.EpEntityComposition.Load();

            var parentEntity = context.EpEntity.Find(Filter.Id);

            if (parentEntity == null) { return Enumerable.Empty<ComponentLinkDto>().AsQueryable(); }

            IQueryable<ComponentLinkDto> q = null;
            if (parentEntity.HierarchyType == HierarchyType.Atomic)
            {
                q = context.EpEntity.Where(e => e.ParentId == parentEntity.Id).Select(e => new ComponentLinkDto
                {
                    Id = e.Id,
                    Code = e.Code,
                    HierarchyType = (int)e.HierarchyType,
                    Name = e.Name,
                    IntermediateEntityId = null
                });
            }
            else
            {
                q = context.EpEntityComposition.Where(e => e.ComposedEntity.Id == Filter.Id).Select(e => new ComponentLinkDto
                {
                    Id = e.Component.Id,
                    Code = e.Component.Code,
                    HierarchyType = (int)e.Component.HierarchyType,
                    Name = e.Component.Name,
                    IntermediateEntityId = e.Id
                });
            }
            return q;
        }
    }

    public class ClassificationQuery : IdFilteredQuery<ClassificationDto>
    {
        public ClassificationQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<ClassificationDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();
            context.EpEntity.Load();
            context.EpClassification.Load();
            return context.EpEntityClassification.Where(e => e.Entity.Id == Filter.Id).Select(e => new ClassificationDto
            {
                Id = e.Classification.Id,
                Name = e.Classification.Name,
                Type = e.Classification.Type,
                IntermediateEntityId = e.Id
            });
        }
    }

    public class OrganismQuery : IdFilteredQuery<EntityOrganismDto>
    {
        public OrganismQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<EntityOrganismDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();
            context.EpEntity.Load();
            context.EpOrganism.Load();
            return context.EpEntityOrganism.Where(e => e.EntityId == Filter.Id).Select(e => new EntityOrganismDto
            {
                Id = e.Organism.Id,
                Name = e.Organism.Name,
                Code = e.Organism.Code,
                GeneGroup = e.GeneGroup,
                IntermediateEntityId = e.Id
            });
        }
    }

    public class NoteQuery : IdFilteredQuery<EntityNoteDto>
    {
        public NoteQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<EntityNoteDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();
            context.EpUser.Load();
            context.EpEntity.Load();
            return context.EpEntityNote.Where(e => e.Entity.Id == Filter.Id).Select(e => new EntityNoteDto
            {
                Id = e.Id,
                Text = e.Text,
                Inserted = e.Inserted,
                Updated = e.Updated,
                UserName = e.User.Name,
                IntermediateEntityId = e.Id
            });
        }
    }
}
