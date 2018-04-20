using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Queries
{
    public class NoteQuery : OneToManyQuery<ApiEntity, EntityNoteDto>
    {
        public NoteQuery(IRepository<ApiEntity, int> parentEntityRepository) 
            : base(parentEntityRepository)
        {
        }

        protected override Task<IQueryable<EntityNoteDto>> GetQueriableAsync(CancellationToken cancellationToken)
        {
            //no api
            return Task.FromResult(Enumerable.Empty<EntityNoteDto>().AsQueryable());
        }
    }
}
