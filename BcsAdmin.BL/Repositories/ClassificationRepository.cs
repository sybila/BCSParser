using BcsAdmin.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BcsAdmin.BL.Repositories
{
    public class EntityClassificationRepository : EntityFrameworkRepository<EpEntityClassification, int>
    {
        public EntityClassificationRepository(IUnitOfWorkProvider unitOfWorkProvider, IDateTimeProvider dateTimeProvider)
            : base(unitOfWorkProvider, dateTimeProvider)
        {
        }
    }

    public class EntityLocationRepository : EntityFrameworkRepository<EpEntityLocation, int>
    {
        public EntityLocationRepository(IUnitOfWorkProvider unitOfWorkProvider, IDateTimeProvider dateTimeProvider) 
            : base(unitOfWorkProvider, dateTimeProvider)
        {
        }
    }

    public class EntityNoteRepository : EntityFrameworkRepository<EpEntityNote, int>
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public EntityNoteRepository(IUnitOfWorkProvider unitOfWorkProvider, IDateTimeProvider dateTimeProvider)
            : base(unitOfWorkProvider, dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        public override void Insert(EpEntityNote entity)
        {
            entity.Inserted = dateTimeProvider.Now;
            base.Insert(entity);
        }

        public override void Update(EpEntityNote entity)
        {
            entity.Updated = dateTimeProvider.Now;
            base.Update(entity);
        }
    }

    public class EntityComponentRepository : EntityFrameworkRepository<EpEntityComposition, int>
    {
        public EntityComponentRepository(IUnitOfWorkProvider unitOfWorkProvider, IDateTimeProvider dateTimeProvider)
            : base(unitOfWorkProvider, dateTimeProvider)
        {
        }
    }

    public class ClassificationRepository : EntityFrameworkRepository<EpClassification, int>
    {
        public ClassificationRepository(IUnitOfWorkProvider unitOfWorkProvider, IDateTimeProvider dateTimeProvider)
            : base(unitOfWorkProvider, dateTimeProvider)
        {
        }
    }
}
