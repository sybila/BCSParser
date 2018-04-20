using AutoMapper;
using BcsAdmin.BL.Dto;
using BcsAdmin.DAL.Api;
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
    public class BiochemicalEntityFacade : FilteredCrudFacadeBase<ApiEntity, int, BiochemicalEntityRowDto, BiochemicalEntityDetailDto, BiochemicalEntityFilter>, IListFacade<BiochemicalEntityRowDto, BiochemicalEntityFilter>
    {
        public BiochemicalEntityFacade(
            IUnitOfWorkProvider unitOfWorkProvider,
            Func<IFilteredQuery<BiochemicalEntityRowDto, BiochemicalEntityFilter>> queryFactory,
            IRepository<ApiEntity, int> repository,
            IEntityDTOMapper<ApiEntity, BiochemicalEntityDetailDto> mapper)
            : base(queryFactory, repository, mapper)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
        }
    }
}