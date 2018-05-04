using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Filters;
using System;

namespace BcsAdmin.BL.Queries
{
    public abstract class OneToManyQuery<TParentEntity, TEntityDto> : AppApiQuery<TEntityDto>, IFilteredQuery<TEntityDto, IdFilter>
       where TParentEntity : IEntity<int>
    {
        public IdFilter Filter { get; set; }

        protected abstract IRepository<TParentEntity, int> GetParentRepository();
    }
}
