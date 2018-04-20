using System;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.DAL.Api;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;

namespace BcsAdmin.BL.Facades
{
    public class StateGridFacade : FilteredCrudFacadeBase<ApiEntity, int, StateEntityDto, StateEntityDto, IdFilter>, IGridFacade<StateEntityDto>
    {
        public StateGridFacade(
            IUnitOfWorkProvider unitOfWorkProvider,
            Func<IFilteredQuery<StateEntityDto, IdFilter>> queryFactory, 
            IRepository<ApiEntity, int> repository, 
            IEntityDTOMapper<ApiEntity, StateEntityDto> mapper) 
            : base(queryFactory, repository, mapper)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
        }
    }
}
