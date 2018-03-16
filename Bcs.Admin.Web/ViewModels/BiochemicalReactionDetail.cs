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

namespace Bcs.Admin.Web.ViewModels
{
    public class BiochemicalReactionDetail : DetailBase<BiochemicalReactionDetailDto>
    {
        private readonly TextPresenter textPresenter;

        [Bind(Direction.None)]
        protected ReactionFacade ReactionFacade => (ReactionFacade)Facade;

        [CodeEditor(nameof(UpdateEquation))]
        [Display(GroupName = "Fields")]
        public string Equation { get; set; }

        [CodeEditor(nameof(UpdateModifier))]
        [Display(GroupName = "Fields")]
        public string Modifier { get; set; }

        public BiochemicalReactionDetail(
            ReactionFacade reactionFacade,
            IMapper mapper,
            IEditableLinkGrid<LocationLinkDto, EntitySuggestionQuery> locationGrid, 
            IEditableLinkGrid<ClassificationDto, ClassificationSuggestionQuery> classificationGrid, 
            IEditableLinkGrid<EntityOrganismDto, OrganismSuggestionQuery> organisms, 
            IEditableGrid<EntityNoteDto> noteGrid) 
            : base(reactionFacade, mapper, locationGrid, classificationGrid, organisms, noteGrid)
        {
            textPresenter = new TextPresenter();
        }

        public void UpdateEquation()
        {
            var equationText = textPresenter.ToRawText(Equation);

            Equation = textPresenter.CreateRichText(equationText, ReactionFacade.GetClassificationSpans(equationText));
        }

        public void UpdateModifier()
        {
            var equationText = textPresenter.ToRawText(Modifier);

            Modifier = textPresenter.CreateRichText(equationText, ReactionFacade.GetClassificationSpans(equationText));
        }

        public override Task PreRender()
        {
            UpdateEquation();
            UpdateModifier();
            return base.PreRender();
        }
    }
}
