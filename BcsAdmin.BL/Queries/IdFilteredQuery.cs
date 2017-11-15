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
    public abstract class IdFilteredQuery<TEntityDto> : EntityFrameworkQuery<TEntityDto, AppDbContext>, IFilteredQuery<TEntityDto, IdFilter>
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
            Context.EpEntity.Load();
            return Context.EpEntityLocation.Where(e => e.Entity.Id == Filter.Id).Select(e => new LocationLinkDto
            {
                Id = e.Entity.Id,
                Code = e.Entity.Code,
                HierarchyType = (int)e.Entity.HierarchyType,
                Name = e.Entity.Name
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
            Context.EpEntity.Load();
            Context.EpEntityComposition.Load();

            var parentEntity = Context.EpEntity.Find(Filter.Id);

            if (parentEntity == null) { return Enumerable.Empty<ComponentLinkDto>().AsQueryable(); }

            IQueryable<EpEntity> q = null;
            if (parentEntity.HierarchyType == HierarchyType.Atomic)
            {
                q = Context.EpEntity.Where(e => e.ParentId == parentEntity.Id);
            }
            else
            {
                q = Context.EpEntityComposition.Where(e => e.ComposedEntity.Id == Filter.Id).Select(e => e.Component);
            }

            return q.Select(e => new ComponentLinkDto
            {
                Id = e.Id,
                Code = e.Code,
                HierarchyType = (int)e.HierarchyType,
                Name = e.Name
            });
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
            Context.EpEntity.Load();
            Context.EpClassification.Load();
            return Context.EpEntityClassification.Where(e => e.Entity.Id == Filter.Id).Select(e => new ClassificationDto
            {
                Id = e.Classification.Id,
                Name = e.Classification.Name,
                Type = e.Classification.Type
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
            Context.EpUser.Load();
            Context.EpEntity.Load();
            return Context.EpEntityNote.Where(e => e.Entity.Id == Filter.Id).Select(e => new EntityNoteDto
            {
                Id = e.Id,
                Text = e.Text,
                Inserted = e.Inserted,
                Updated = e.Updated,
                UserName = e.User.Name
            });
        }
    }
}
