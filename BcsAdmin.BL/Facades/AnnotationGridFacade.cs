using System;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Repositories.Api;
using BcsAdmin.DAL.Api;

namespace BcsAdmin.BL.Facades
{
    public class AnnotationGridFacade : DependantGridFacade<ApiAnnotation, AnnotationDto>
    {
        public AnnotationGridFacade(Func<IFilteredQuery<AnnotationDto, IdFilter>> queryFactory,
            Func<IRepository<ApiAnnotation, int>> repositoryFactory,
            IEntityDTOMapper<ApiAnnotation, AnnotationDto> mapper)
            : base(queryFactory, repositoryFactory, mapper)
        {
        }

        protected override IRepository<ApiAnnotation, int> GetRepository(int parentId, string parentRepositoryName)
        {
            var r = RepositoryFactory().CastAs<AnnotationRepository>();
            r.RepoName = $"{parentRepositoryName}/{parentId}/annotations";
            return r;
        }
    }
}
