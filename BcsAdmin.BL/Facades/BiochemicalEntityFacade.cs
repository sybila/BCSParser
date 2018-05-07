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
using BcsAdmin.BL.Repositories.Api;

namespace BcsAdmin.BL.Facades
{
    public class BiochemicalEntityFacade : AsyncCrudFacadeBase<ApiEntity, int, BiochemicalEntityRowDto, BiochemicalEntityDetailDto, BiochemicalEntityFilter>
    {
        public BiochemicalEntityFacade(
            Func<IFilteredQuery<BiochemicalEntityRowDto, BiochemicalEntityFilter>> queryFactory,
            IAsyncRepository<ApiEntity, int> repository,
            IEntityDTOMapper<ApiEntity, BiochemicalEntityDetailDto> mapper)
            : base(queryFactory, repository, mapper)
        {
        }
    }
}