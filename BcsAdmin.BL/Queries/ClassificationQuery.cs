using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;

namespace BcsAdmin.BL.Queries
{
    public class ClassificationQuery : EntityFrameworkQuery<ClassificationDto, AppDbContext>, IFilteredQuery<ClassificationDto, IdFilter>
    {
        protected ClassificationQuery(IUnitOfWorkProvider unitOfWorkProvider) 
            : base(unitOfWorkProvider)
        {
        }

        public IdFilter Filter { get; set; }

        protected override IQueryable<ClassificationDto> GetQueryable()
        {
            return Context.EpEntityClassification.Where(e => e.EntityId == Filter.Id).Select(s=> new ClassificationDto {
                Id = s.Entity.Id,
                Name = s.Entity.Name,
                Type = s.Entity.Type
            });
        }
    }
}
