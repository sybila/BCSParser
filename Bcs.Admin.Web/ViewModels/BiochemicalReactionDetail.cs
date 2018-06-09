using BcsAdmin.BL.Dto;
using System.ComponentModel.DataAnnotations;
using Bcs.Admin.Web.ViewModels.Grids;
using BcsAdmin.BL.Facades;
using AutoMapper;
using BcsAdmin.BL.Queries;
using Bcs.Admin.BL.Dto;
using Bcs.Admin.Web.Controls.Dynamic;
using System.Collections.Generic;
using BcsResolver.Syntax.Tokenizer;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using System.Linq;

namespace Bcs.Admin.Web.ViewModels
{
    public class BiochemicalReactionDetail : DetailBase<BiochemicalReactionDetailDto>
    {
        [Bind(Direction.None)]
        protected ReactionFacade ReactionFacade => (ReactionFacade)Facade;

        [Required]
        [CodeEditor(nameof(UpdateEquationAsync))]
        [Display(GroupName = "Fields")]
        public string Equation { get; set; }

        [Bind(Direction.ClientToServer)]
        public string EquationText { get; set; } = "";

        public List<StyleSpan> EquationSpans { get; set; }

        [CodeEditor(nameof(UpdateModifierAsync))]
        [Display(GroupName = "Fields")]
        [Bind(Direction.None)]
        public string Modifier { get; set; }

        public List<StyleSpan> ModifierSpans { get; set; }

        [Display(GroupName = "Fields", Name = "Name")]
        public string Name { get; set; }

        public IList<string> EquationErrors { get; set; }

        public BiochemicalReactionDetail(
            ReactionFacade reactionFacade,
            IMapper mapper,
            IEditableLinkGrid<LocationLinkDto, EntitySuggestionQuery> locationGrid, 
            IEditableLinkGrid<ClassificationDto, ClassificationSuggestionQuery> classificationGrid, 
            IEditableLinkGrid<OrganismDto, OrganismSuggestionQuery> organisms,
            IEditableGrid<int, AnnotationDto> annotationGrid,
            IEditableGrid<int, EntityNoteDto> noteGrid) 
            : base(reactionFacade, mapper, annotationGrid, classificationGrid, organisms, noteGrid)
        {
            Organisms.ParentRepositoryName = "rules";
            Classifications.ParentRepositoryName = "rules";
            Annotations.ParentRepositoryName = "rules";
            Notes.ParentRepositoryName = "rules";
        }

        protected override void AfterMap(BiochemicalReactionDetailDto dto)
        {
            dto.Equation = dto.Equation;
            dto.Modifier = dto.Modifier;
        }

        [AllowStaticCommand]
        public async Task<List<StyleSpan>> UpdateEquationAsync()
        {
            await ExecuteSafeAsync(async () =>
            {
                var model = await ReactionFacade.GetReactionModelAsync(EquationText ?? "");
                EquationSpans = ReactionFacade.GetClassificationSpans(model);

                EquationErrors = model?.Errors.Select(e => e.Message).ToList() ?? new List<string>();
            });
            return EquationSpans;
        }

        public async Task UpdateModifierAsync()
        {
            await ExecuteSafeAsync(async () =>
            {
                var model = await ReactionFacade.GetReactionModelAsync(Modifier);
                ModifierSpans = ReactionFacade.GetClassificationSpans(model);
            });
        }

        public override async Task PoputateGridsAsync()
        {
            await ExecuteSafeAsync(async () =>
            {
                Classifications.EntitySearchSelect.Filter.Category = CategoryType.Rule;

                await base.PoputateGridsAsync();
            });
        }

        public async override Task PreRender()
        {
            await UpdateEquationAsync();
            //await UpdateModifierAsync();
            await base.PreRender();
        }
    }
}
