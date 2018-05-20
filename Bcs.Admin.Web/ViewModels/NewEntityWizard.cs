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
        private readonly AnnotationGridFacade annotationGridFacade;

        public string Code { get; set; }
        public string Name { get; set; }
        public BiochemicalEntityDetailDto NewEntity { get; set; }
        public bool IsVisible { get; set; }
        public bool IsSearchDone { get; set; }
        public GridViewDataSet<SimilarEntityDto> SimilarResults { get; set; }
        public SimilarEntityDto SelectedSimilarEntity { get; set; }

        private Dashboard parent;

        public NewEntityWizard(AdvancedEntitySearchFacade searchFacade, BiochemicalEntityFacade biochemicalEntityFacade, AnnotationGridFacade annotationGridFacade)
        {
            this.searchFacade = searchFacade;
            this.biochemicalEntityFacade = biochemicalEntityFacade;
            this.annotationGridFacade = annotationGridFacade;
        }

        public void SetParent(Dashboard parent)
        {
            this.parent = parent;
        }

        public void StartNew()
        {
            IsVisible = true;
            IsSearchDone = false;
            SelectedSimilarEntity = null;
        }

        public void Close()
        {
            IsVisible = false;
            IsSearchDone = false;
            NewEntity = null;
            SelectedSimilarEntity = null;
            Code = "";
            Name = "";
        }

        public async Task NextAsync()
        {
            await searchFacade.FillSimilarEntitySearchAsync(SimilarResults, new SimilarEntitySearchFilter { Code = Code, Name = Name });
            IsSearchDone = true;
        }

        public void CreateNewBlank()
        {
            SelectedSimilarEntity = null;
            NewEntity = new BiochemicalEntityDetailDto
            {
                Code = Code,
                Name = Name
            };
        }

        public void SelectEntity(SimilarEntityDto similarEntityDto)
        {
            SelectedSimilarEntity = similarEntityDto;
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

                if (SelectedSimilarEntity?.IsExternal == true)
                {
                    await annotationGridFacade.SaveAsync(
                        entity.Id,
                        "entities",
                        new AnnotationDto
                        {
                            Code = SelectedSimilarEntity.Code,
                            Type = SelectedSimilarEntity.Database
                        });
                }
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
    }
}