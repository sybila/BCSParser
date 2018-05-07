using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Filters;
using System;
using BcsAdmin.BL.Repositories.Api;

namespace BcsAdmin.BL.Queries
{
    public abstract class OneToManyQuery<TParentEntity, TEntityDto> : AppApiQuery<TEntityDto>, IFilteredQuery<TEntityDto, IdFilter>
       where TParentEntity : IEntity<int>
    {
        public IdFilter Filter { get; set; }

        protected abstract IAsyncRepository<TParentEntity, int> GetParentRepository();
    }
}
