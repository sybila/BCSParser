using AutoMapper;
using BcsAdmin.BL.Dto;
using BcsAdmin.DAL.Models;
using DotVVM.Framework.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riganti.Utils.Infrastructure.Services.Facades;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System.Linq.Expressions;
using BcsAdmin.BL.Queries;
using BcsAdmin.BL.Filters;
using Bcs.Admin.BL.Dto;

namespace BcsAdmin.BL.Facades
{
    public class BiochemicalEntityFacade : FilteredCrudFacadeBase<EpEntity, int, BiochemicalEntityRowDto, BiochemicalEntityDetailDto, BiochemicalEntityFilter>, IListFacade<BiochemicalEntityRowDto, BiochemicalEntityFilter>
    {
        public BiochemicalEntityFacade(
            IUnitOfWorkProvider unitOfWorkProvider,
            Func<IFilteredQuery<BiochemicalEntityRowDto, BiochemicalEntityFilter>> queryFactory,
            IRepository<EpEntity, int> repository,
            IEntityDTOMapper<EpEntity, BiochemicalEntityDetailDto> mapper)
            : base(queryFactory, repository, mapper)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
        }

        public override BiochemicalEntityDetailDto GetDetail(int id)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var includes = new IIncludeDefinition<EpEntity>[] {
                    Includes.For<EpEntity>()
                    .Include(e => e.Locations)
                    .Then<EpEntity, ICollection<EpEntityLocation>, EpEntityLocation, EpEntity>(e => e.Location),
                    Includes.For<EpEntity>()
                    .Include(e=> e.Components)
                    .Then<EpEntity, ICollection<EpEntityComposition>, EpEntityComposition, EpEntity>(e => e.Component),
                    Includes.For<EpEntity>()
                    .Include(e=> e.Classifications)
                    .Then<EpEntity, ICollection<EpEntityClassification>, EpEntityClassification, EpClassification>(e => e.Classification),
                    Includes.For<EpEntity>()
                    .Include(e=> e.Notes)
                    .Then<EpEntity, ICollection<EpEntityNote>, EpEntityNote, EpUser>(e => e.User) };

                var entity = Repository.GetById(id, includes);

                return Mapper.MapToDTO(entity);
            }
        }
    }
}
