using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using BcsAdmin.BL.Repositories.Api;
using System;

namespace BcsAdmin.BL.Queries
{
    public class ClassificationQuery : ManyToManyQuery<ClassificationArray, ApiClassification, ClassificationDto>
    {
        private readonly Func<IAsyncRepository<ClassificationArray, int>> parentEntityRepositoryFunc;

        public ClassificationQuery(Func<IAsyncRepository<ClassificationArray, int>> parentEntityRepositoryFunc, IAsyncRepository<ApiClassification, int> associatedEntityRepository)
            : base(associatedEntityRepository)
        {
            this.parentEntityRepositoryFunc = parentEntityRepositoryFunc;
        }

        protected override IQueryable<ClassificationDto> ProcessEntities(IQueryable<ApiClassification> q, ClassificationArray parentEntity)
        {
            return q
                .Select(e => new ClassificationDto
            {
                Id = e.Id,
                Name = e.Name,
                Type = (int)e.Type,
                IntermediateEntityId = parentEntity.Id
            });
        }

        protected override IList<int> GetAssocitedEntityIds(ClassificationArray parent)
        {
            return parent.Classifications;
        }

        protected override IAsyncRepository<ClassificationArray, int> GetParentRepository()
        {
            var parentRepo = parentEntityRepositoryFunc().CastAs<ClassificationArrayRepository>();
            parentRepo.RepoName = Filter.ParentEntityType;
            return parentRepo;
        }
    }
}
