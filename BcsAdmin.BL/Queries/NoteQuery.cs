using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System;
using BcsAdmin.BL.Filters;

namespace BcsAdmin.BL.Queries
{
    public class NoteQuery : AppApiQuery<EntityNoteDto>, IFilteredQuery<EntityNoteDto, IdFilter>
    {
        public IdFilter Filter { get; set; }

        protected async override Task<IQueryable<EntityNoteDto>> GetQueriableAsync (CancellationToken cancellationToken)
        {
            if(Filter.Id == 0)
            {
                return new EntityNoteDto[] { }.AsQueryable();
            }

            var results = await GetWebDataAsync<ApiNote>(cancellationToken, $"{Filter.ParentEntityType}/{Filter.Id}/notes");

            return results.Select(n => new EntityNoteDto
            {
                Text = n.Text,
                UserName = n.UserName,
                Id = n.Id,
                Inserted = n.Inserted == null ? (DateTime?) null : DateTime.Parse(n.Inserted),
                Updated = n.Updated == null ? (DateTime?) null : DateTime.Parse(n.Updated)
            })
            .AsQueryable();
        }
    }
}
