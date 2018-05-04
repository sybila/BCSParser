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
using Bcs.Analyzer.DemoWeb.Utils;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using System.Linq;

namespace Bcs.Admin.Web.ViewModels
{
    public class BiochemicalReactionDetail : DetailBase<BiochemicalReactionDetailDto>
    {
        private readonly TextPresenter textPresenter;

        [Bind(Direction.None)]
        protected ReactionFacade ReactionFacade => (ReactionFacade)Facade;

        [CodeEditor(nameof(UpdateEquationAsync))]
        [Display(GroupName = "Fields")]
        public string Equation { get; set; }

        [CodeEditor(nameof(UpdateModifierAsync))]
        [Display(GroupName = "Fields")]
        public string Modifier { get; set; }

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
            textPresenter = new TextPresenter();
            Organisms.ParentRepositoryName = "rules";
            Classifications.ParentRepositoryName = "rules";
            Annotations.ParentRepositoryName = "rules";
            Notes.ParentRepositoryName = "rules";
        }

        public async Task UpdateEquationAsync()
        {
            var equationText = textPresenter.ToRawText(Equation);
            var model = await ReactionFacade.GetReactionModelAsync(equationText);
            var spans = ReactionFacade.GetClassificationSpans(model);

            Equation = textPresenter.CreateRichText(equationText, spans);
            EquationErrors = model?.Errors.Select(e => e.Message).ToList() ?? new List<string>();
        }

        public async Task UpdateModifierAsync()
        {
            var equationText = textPresenter.ToRawText(Modifier);

            var model = await ReactionFacade.GetReactionModelAsync(equationText);

            Modifier = textPresenter.CreateRichText(equationText, ReactionFacade.GetClassificationSpans(model));
        }

        public override Task PoputateGridsAsync()
        {
            Classifications.EntitySearchSelect.Filter.Category = CategoryType.Rule;

            return base.PoputateGridsAsync();
        }

        public async override Task PreRender()
        {
            await UpdateEquationAsync();
            await UpdateModifierAsync();
            await base.PreRender();
        }
    }
}
