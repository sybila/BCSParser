using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Queries
{
    public class StateEntityQuery : OneToManyQuery<ApiEntity, StateEntityDto>
    {
        private readonly IRepository<ApiEntity, int> parentEntityRepository;

        public StateEntityQuery(IRepository<ApiEntity, int> parentEntityRepository) : base()
        {
            this.parentEntityRepository = parentEntityRepository;
        }

        protected override IRepository<ApiEntity, int> GetParentRepository() => parentEntityRepository;

        protected async override Task<IQueryable<StateEntityDto>> GetQueriableAsync(CancellationToken cancellationToken)
        {
            var parentRepo = GetParentRepository();
            var parent = await parentRepo.GetByIdAsync(cancellationToken, Filter.Id);

            return (parent?.States 
                ??  new List<ApiState> {})
                .Select(e => new StateEntityDto
                {
                    Id = e.Code,
                    Description = e.Description
                })
                .AsQueryable();
        }
    }
}
