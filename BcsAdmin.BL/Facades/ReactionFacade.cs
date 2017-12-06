using System;
using System.Collections.Generic;
using System.Text;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;

namespace BcsAdmin.BL.Facades
{
    public class ReactionFacade : FilteredCrudFacadeBase<EpReaction, int, ReactionRowDto, BiochemicalEntityDetailDto, ReactionFilter>, IListFacade<ReactionRowDto, ReactionFilter>
    {
        public ReactionFacade(
            Func<IFilteredQuery<ReactionRowDto, ReactionFilter>> queryFactory, 
            IRepository<EpReaction, int> repository, 
            IEntityDTOMapper<EpReaction, BiochemicalEntityDetailDto> mapper,
            IUnitOfWorkProvider unitOfWorkProvider) 
            : base(queryFactory, repository, mapper)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
        }
    }
}
