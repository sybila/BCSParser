using System;
using BcsAdmin.BL.Dto;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using DotVVM.Framework.ViewModel;

namespace Bcs.Admin.Web.ViewModels
{
    public class NewEntityWizard : AppViewModelBase
    {
        private readonly AdvancedEntitySearchFacade searchFacade;
        private readonly BiochemicalEntityFacade biochemicalEntityFacade;

        public string Code { get; set; }
        public BiochemicalEntityDetailDto NewEntity { get; set; }
        public bool IsVisible { get; set; }
        public GridViewDataSet<SimilarEntityDto> SimilarResults { get; set; }
        private Dashboard parent;

        public NewEntityWizard(AdvancedEntitySearchFacade searchFacade, BiochemicalEntityFacade biochemicalEntityFacade)
        {
            this.searchFacade = searchFacade;
            this.biochemicalEntityFacade = biochemicalEntityFacade;
        }

        public void SetParent(Dashboard parent)
        {
            this.parent = parent;
        }

        public void StartNew()
        {
            IsVisible = true;
            IsSearchDone = false;
        }

        public void Close()
        {
            IsVisible = false;
            IsSearchDone = false;
            NewEntity = null;
            Code = "";
        }

        public void Next()
        {
            searchFacade.FillSimilarEntitySearch(SimilarResults, new SimilarEntitySearchFilter { Name = Code });
            IsSearchDone = true;
        }

        public void CreateNewBlank()
        {
            NewEntity = new BiochemicalEntityDetailDto
            {
                Code = Code
            };
        }

        public void SelectEntity(SimilarEntityDto similarEntityDto)
        {
            NewEntity = new BiochemicalEntityDetailDto
            {
                Code = similarEntityDto.Code,
                Name = similarEntityDto.Name
            };
        }

        public async Task SaveAsync()
        {
            await ExecuteSafeAsync(async () =>
            {
                var entity = await biochemicalEntityFacade.SaveAsync(NewEntity);
                Close();
                await parent.EditEntityAsync(entity.Id);
            });
        }

        public override Task Init()
        {
            SimilarResults = new GridViewDataSet<SimilarEntityDto>()
            {
                PagingOptions =
                {
                    PageSize = 10
                },
                SortingOptions = new SortingOptions
                {
                    SortExpression = "Code"
                }
            };
            return base.Init();
        }

        public bool IsSearchDone { get; set; }
    }
}