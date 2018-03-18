using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.BL.Filters;

namespace BcsAdmin.BL.Facades
{
    public interface IGridFacade<TEntityDto> : ICrudFilteredFacade<TEntityDto, TEntityDto, IdFilter, int>
          where TEntityDto : IEntity<int>
    {

    }
}
