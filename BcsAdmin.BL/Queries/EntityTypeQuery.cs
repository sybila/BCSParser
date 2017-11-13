using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Queries
{
    public class EntityTypeQuery : EntityFrameworkQuery<string, AppDbContext>
    {
        public EntityTypeQuery(IUnitOfWorkProvider unitOfWorkProvider) 
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<string> GetQueryable()
        {
            return Context.EpEntity
                .Select(e => e.HierarchyType.ToString("F"))
                .Distinct();
        }
    }
}
