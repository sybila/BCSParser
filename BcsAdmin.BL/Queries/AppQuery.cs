using DotVVM.Framework.Controls;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Queries
{
    public abstract class AppQuery<TResult> : EntityFrameworkQuery<TResult>
    {
        protected AppQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        public virtual GridViewDataSetLoadedData Execute(IGridViewDataSetLoadOptions loadOptions)
        {
            return loadOptions.GetDataFromQueryable(GetQueryable());
        }
    }
}
