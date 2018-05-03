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

            var results = await GetWebDataAsync<ApiNote>(cancellationToken, $"entities/{Filter.Id}/notes");

            return results.Select(n => new EntityNoteDto
            {
                Text = n.Text,
                UserName = n.UserName,
                Id = n.Id,
                Inserted = DateTime.Parse(n.Inserted),
                Updated = DateTime.Parse(n.Updated)
            })
            .AsQueryable();
        }
    }

    public class AnnotationQuery : AppApiQuery<AnnotationDto>, IFilteredQuery<AnnotationDto, IdFilter>
    {
        public IdFilter Filter { get; set; }

        protected async override Task<IQueryable<AnnotationDto>> GetQueriableAsync(CancellationToken cancellationToken)
        {
            if (Filter.Id == 0)
            {
                return new AnnotationDto[] { }.AsQueryable();
            }

            var results = await GetWebDataAsync<ApiAnnotation>(cancellationToken, $"entities/{Filter.Id}/annotations");

            return results.Select(n => new AnnotationDto
            {
                Id = n.Id,
                Code = n.TermId,
                Type = n.TermType,
            })
            .AsQueryable();
        }
    }
}
