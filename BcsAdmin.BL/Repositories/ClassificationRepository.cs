using BcsAdmin.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;


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
        public EntityNoteRepository(IUnitOfWorkProvider unitOfWorkProvider, IDateTimeProvider dateTimeProvider)
            : base(unitOfWorkProvider, dateTimeProvider)
        {
        }
    }

    public class EntityComponentRepository : EntityFrameworkRepository<EpEntityComposition, int>
    {
        public EntityComponentRepository(IUnitOfWorkProvider unitOfWorkProvider, IDateTimeProvider dateTimeProvider)
            : base(unitOfWorkProvider, dateTimeProvider)
        {
        }
    }
}
