using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BcsAdmin.BL.Facades
{
    public class BasicListFacade : FacadeBase
    {
        private readonly Func<EntityTypeNamesQuery> entityTypeQueryFunc;

        public BasicListFacade(IUnitOfWorkProvider unitOfWorkProvider, Func<EntityTypeNamesQuery> entityTypeQueryFunc)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
            this.entityTypeQueryFunc = entityTypeQueryFunc;
        }

        public IList<string> GetEntityTypeNames()
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var q = entityTypeQueryFunc();
                return q.Execute();
            }
        }

        public List<BiochemicalEntityTypeDto> GetEntityTypes()
        {
            return
                Enum.GetValues(typeof(HierarchyType))
                .Cast<HierarchyType>()
                .Select(v => new BiochemicalEntityTypeDto
                {
                    Id = (int)v,
                    Name = v.ToString("F")
                })
                .ToList();
        }
    }
}
