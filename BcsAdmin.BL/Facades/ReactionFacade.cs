using System;
using System.Collections.Generic;
using System.Text;
using Bcs.Admin.BL.Dto;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Services.Facades;

namespace BcsAdmin.BL.Facades
{
    public class ReactionFacade : FilteredCrudFacadeBase<EpReaction, int, ReactionRowDto, BiochemicalReactionDetailDto, ReactionFilter>, IListFacade<ReactionRowDto, ReactionFilter>
    {
        public ReactionFacade(
            Func<IFilteredQuery<ReactionRowDto, ReactionFilter>> queryFactory, 
            IRepository<EpReaction, int> repository, 
            IEntityDTOMapper<EpReaction, BiochemicalReactionDetailDto> mapper,
            IUnitOfWorkProvider unitOfWorkProvider) 
            : base(queryFactory, repository, mapper)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
        }
    }
}
