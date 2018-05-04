using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using System.Threading;
using System.Threading.Tasks;
using BcsAdmin.BL.Filters;

namespace BcsAdmin.BL.Queries
{
    public class AnnotationQuery : AppApiQuery<AnnotationDto>, IFilteredQuery<AnnotationDto, IdFilter>
    {
        public IdFilter Filter { get; set; }

        protected async override Task<IQueryable<AnnotationDto>> GetQueriableAsync(CancellationToken cancellationToken)
        {
            if (Filter.Id == 0)
            {
                return new AnnotationDto[] { }.AsQueryable();
            }

            var results = await GetWebDataAsync<ApiAnnotation>(cancellationToken, $"{Filter.ParentEntityType}/{Filter.Id}/annotations");

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
