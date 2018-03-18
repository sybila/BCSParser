using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Filters;

namespace BcsAdmin.BL.Queries
{
    public abstract class IdFilteredQuery<TEntityDto> : AppQuery<TEntityDto>, IFilteredQuery<TEntityDto, IdFilter>
    {
        public IdFilteredQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        public IdFilter Filter { get; set; }
    }
}
