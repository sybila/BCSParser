using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Queries;
using BcsAdmin.DAL.Api;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using BcsAdmin.BL.Filters;
using DotVVM.Framework.Controls;
using BcsAdmin.BL.Repositories.Api;
using System.Threading;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Facades
{
    public class AdvancedEntitySearchFacade : FacadeBase
    {
        private readonly Func<OneToManyQuery<ApiEntity, EntityUsageDto>> entityUsagesQuery;
        private readonly IAsyncReadonlyRepository<ApiEntity, string> asyncReadonlyRepository;

        public AdvancedEntitySearchFacade(
            IUnitOfWorkProvider unitOfWorkProvider,
            Func<OneToManyQuery<ApiEntity, EntityUsageDto>> entityUsagesQuery,
            IAsyncReadonlyRepository<ApiEntity, string> asyncReadonlyRepository)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
            this.entityUsagesQuery = entityUsagesQuery;
            this.asyncReadonlyRepository = asyncReadonlyRepository;
        }

        public async Task<List<string>> GetEntityUsageListAsync(int entityId)
        {
            var query = entityUsagesQuery();
            query.Filter = new Filters.IdFilter { Id = entityId };
            var usagesList = await query.ExecuteAsync();
            return usagesList.Select(u => $"{u.CategoryType}: {u.FullName}").ToList();
        }

        public async Task FillSimilarEntitySearchAsync(GridViewDataSet<SimilarEntityDto> dataSet, SimilarEntitySearchFilter similarEntitySearchFilter)
        {
            var localEntity = await asyncReadonlyRepository.GetByIdAsync(CancellationToken.None, similarEntitySearchFilter?.Code ?? "");

            IEnumerable<SimilarEntityDto> testData = localEntity != null
                ? new[]
                  {
                        new SimilarEntityDto{ Id = localEntity.Id, Code = localEntity.Code, Name = localEntity.Name},
                  }
                : new SimilarEntityDto[] { };

            testData = testData.Concat(new[] {
                new SimilarEntityDto{ Name="Dummy1",Code="Dummy1", Id=3},
                new SimilarEntityDto{ Name="Dummy2",Code="Dummy2", Database="kegg", Link="http://database.db" }
            });

            dataSet.PagingOptions.TotalItemsCount = 3;
            dataSet.Items = dataSet.Items = testData.ToList();
        }
    }
}
