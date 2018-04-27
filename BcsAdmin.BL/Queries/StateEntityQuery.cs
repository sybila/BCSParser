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
        public StateEntityQuery(IRepository<ApiEntity, int> parentEntityRepository) : base(parentEntityRepository)
        {
        }

        protected async override Task<IQueryable<StateEntityDto>> GetQueriableAsync(CancellationToken cancellationToken)
        {
            var parent = await ParentEntityRepository.GetByIdAsync(cancellationToken, Filter.Id);

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
