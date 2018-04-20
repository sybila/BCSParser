using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Filters;

namespace BcsAdmin.BL.Queries
{
    public abstract class OneToManyQuery<TParentEntity, TEntityDto> : AppApiQuery<TEntityDto>, IFilteredQuery<TEntityDto, IdFilter>
       where TParentEntity : IEntity<int>
    {
        public IdFilter Filter { get; set; }
        protected IRepository<TParentEntity, int> ParentEntityRepository { get; }

        public OneToManyQuery(IRepository<TParentEntity, int> parentEntityRepository)
        {
            ParentEntityRepository = parentEntityRepository;
        }
    }
}
