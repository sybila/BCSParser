using BcsAdmin.DAL.Api;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using BcsAdmin.BL.Repositories.Api;

namespace BcsAdmin.BL.Queries
{
    public class OrganismQuery : ManyToManyQuery<OrganismArray, ApiOrganism, OrganismDto>
    {
        private readonly Func<IAsyncRepository<OrganismArray, int>> parentEntityRepositoryFunc;

        public OrganismQuery(Func<IAsyncRepository<OrganismArray, int>> parentEntityRepositoryFunc, 
            IAsyncRepository<ApiOrganism, int> associatedEntityRepository) 
            : base(associatedEntityRepository)
        {
            this.parentEntityRepositoryFunc = parentEntityRepositoryFunc;
        }

        protected override IList<int> GetAssocitedEntityIds(OrganismArray parent)
        {
            return parent.Organisms;
        }

        protected override IAsyncRepository<OrganismArray, int> GetParentRepository()
        {
            var parentRepo = parentEntityRepositoryFunc().CastAs<OrganismArrayRepository>();
            parentRepo.RepoName = Filter.ParentEntityType;
            return parentRepo;
        }

        protected override IQueryable<OrganismDto> ProcessEntities(IQueryable<ApiOrganism> q, OrganismArray parentEntity)
        {
            return q.Select(e => new OrganismDto
             {
                 Id = e.Id,
                 Name = e.Name,
                 Code = "",
                 GeneGroup = "",
                 IntermediateEntityId = parentEntity.Id
             });
        }
    }
}
