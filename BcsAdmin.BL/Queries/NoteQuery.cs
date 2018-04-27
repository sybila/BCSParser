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

        protected async override Task<IQueryable<EntityNoteDto>> GetQueriableAsync (CancellationToken cancellationToken)
        {
            if(Filter.Id == 0)
            {
                return new EntityNoteDto[] { }.AsQueryable();
            }

            var results = await GetWebDataAsync<ApiNote>(cancellationToken, $"entities/{Filter.Id}/notes");

            return results.Select(n => new EntityNoteDto
            {
                Text = n.Text,
                UserName = n.UserName,
                Id = n.Id,
            })
            .AsQueryable();
        }
    }
}
