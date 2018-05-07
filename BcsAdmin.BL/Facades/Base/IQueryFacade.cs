using System;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Facades
{
    public interface IQueryFacade<TListDTO, TFilter>
    {
        Func<IFilteredQuery<TListDTO, TFilter>> QueryFactory { get; }
    }
}