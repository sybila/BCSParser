using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Riganti.Utils.Infrastructure.Core;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace BcsAdmin.BL.Queries
{

    public abstract class ManyToManyQuery<TParentEntity, TAssociatedEntity, TEntityDto> : OneToManyQuery<TParentEntity, TEntityDto>
        where TParentEntity : IEntity<int>
        where TAssociatedEntity : IEntity<int>
    {
        private IRepository<TAssociatedEntity, int> AssociatedEntityRepository { get; }

        public ManyToManyQuery(IRepository<TAssociatedEntity, int> associatedEntityRepository)
            : base()
        {
            AssociatedEntityRepository = associatedEntityRepository;
        }

        protected override async Task<IQueryable<TEntityDto>> GetQueriableAsync(CancellationToken cancellationToken)
        {
            var parentRepo = GetParentRepository();
            var parent = await parentRepo.GetByIdAsync(cancellationToken, Filter.Id);

            if(parent == null) { return (new TEntityDto[] { }).AsQueryable(); }

            var ids = GetAssocitedEntityIds(parent) ?? new int [] { };

            var entities = await AssociatedEntityRepository.GetByIdsAsync(cancellationToken, ids);

            var q = entities.AsQueryable();

            return ProcessEntities(q, parent);
        }

        protected abstract IQueryable<TEntityDto> ProcessEntities(IQueryable<TAssociatedEntity> q, TParentEntity parentEntity);
        protected abstract IList<int> GetAssocitedEntityIds(TParentEntity parent);
    }
}
