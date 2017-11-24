using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Queries
{
    public class EntityTypeNamesQuery : EntityFrameworkQuery<string>
    {
        public EntityTypeNamesQuery(IUnitOfWorkProvider unitOfWorkProvider) 
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<string> GetQueryable()
        {
            return Context.CastTo<AppDbContext>().EpEntity
                .Select(e => e.HierarchyType)
                .Distinct()
                .Select(e=> e.ToString("F"));
                
        }
    }
}
