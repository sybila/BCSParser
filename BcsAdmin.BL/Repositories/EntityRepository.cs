using BcsAdmin.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;

namespace BcsAdmin.BL.Dto.Repositories
{
    public class EntityRepository : EntityFrameworkRepository<EpEntity, int>, IRepository<EpEntity, int>
    {
        public EntityRepository(IUnitOfWorkProvider unitOfWorkProvider, IDateTimeProvider dateTimeProvider)
            : base(unitOfWorkProvider, dateTimeProvider)
        {
        }
    }
}
