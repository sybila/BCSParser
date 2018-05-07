using System;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.DAL.Api;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using AutoMapper;
using System.Collections.Generic;
using BcsAdmin.BL.Repositories.Api;

namespace BcsAdmin.BL.Facades
{
    public class ClassificationGridFacade : DependantLinkGridFacade<ClassificationArray, ApiClassification, ClassificationDto>
    {
        private readonly Func<IAsyncRepository<ClassificationArray, int>> parentRepositoryFunc;

        public ClassificationGridFacade(
            Func<IAsyncRepository<ClassificationArray, int>> parentRepositoryFunc,
            IAsyncRepository<ApiClassification, int> associatedEntityRepository, 
            Func<ManyToManyQuery<ClassificationArray, ApiClassification, ClassificationDto>> queryFactory, 
            IUnitOfWorkProvider unitOfWorkProvider, 
            IMapper mapper) 
            : base(associatedEntityRepository, queryFactory, mapper)
        {
            this.parentRepositoryFunc = parentRepositoryFunc;
        }

        protected override IAsyncRepository<ClassificationArray, int> GetParentRepository(string paentRepositoryName)
        {
            var r = (ClassificationArrayRepository)parentRepositoryFunc();
            r.RepoName = paentRepositoryName;
            return r;
        }

        protected override void UnlinkCore(ClassificationArray parentEntity, int associatedId)
        {
            parentEntity.Classifications.Remove(associatedId);
        }

        internal override void LinkCore(ClassificationArray parentEntity, int associatedId)
        {
            parentEntity.Classifications.Add(associatedId);
        }
    }
}
