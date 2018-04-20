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

namespace BcsAdmin.BL.Facades
{
    public class AdvancedEntitySearchFacade : FacadeBase
    {
        private readonly Func<OneToManyQuery<ApiEntity, EntityUsageDto>> entityUsagesQuery;

        public AdvancedEntitySearchFacade(IUnitOfWorkProvider unitOfWorkProvider, Func<OneToManyQuery<ApiEntity, EntityUsageDto>> entityUsagesQuery)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
            this.entityUsagesQuery = entityUsagesQuery;
        }

        public async System.Threading.Tasks.Task<List<string>> GetEntityUsageListAsync(int entityId)
        {
            var query = entityUsagesQuery();
            query.Filter = new Filters.IdFilter { Id = entityId };
            var usagesList = await query.ExecuteAsync();
            return usagesList.Select(u => $"{u.CategoryType}: {u.FullName}").ToList();
        }

        public void FillSimilarEntitySearch(GridViewDataSet<SimilarEntityDto> dataSet, SimilarEntitySearchFilter similarEntitySearchFilter)
        {
            var testData = new[]
            {
                new SimilarEntityDto{ Name="Test1",Code="Test1", Id=3},
                new SimilarEntityDto{Name="Test2",Code="Test2", Database="Database", Link="http://database.db"}
            };

            dataSet.PagingOptions.TotalItemsCount = 2;
            dataSet.Items = dataSet.Items = testData.ToList();
        }
    }
}
