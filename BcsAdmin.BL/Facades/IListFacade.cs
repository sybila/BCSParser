using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Services.Facades;

namespace BcsAdmin.BL.Facades
{
    public interface IListFacade<TGridDto, TFilter> : ICrudFilteredListFacade<TGridDto, TFilter>, IFacade
    {
    }
}
