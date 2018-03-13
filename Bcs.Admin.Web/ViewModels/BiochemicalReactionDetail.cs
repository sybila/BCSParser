﻿using BcsAdmin.BL.Dto;
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

namespace Bcs.Admin.Web.ViewModels
{
    public class BiochemicalReactionDetail : DetailBase
    {
        private readonly ReactionFacade reactionFacade;
        private readonly TextPresenter textPresenter;

        [CodeEditor(nameof(WriteEquation))]
        [Display(GroupName = "Fields")]
        public string Equation { get; set; }

        [CodeEditor]
        [Display(GroupName = "Fields")]
        public string Modifier { get; set; }

        public BiochemicalReactionDetail(
            ReactionFacade reactionFacade,
            IMapper mapper,
            IEditableLinkGrid<LocationLinkDto, EntitySuggestionQuery> locationGrid, 
            IEditableLinkGrid<ClassificationDto, ClassificationSuggestionQuery> classificationGrid, 
            IEditableLinkGrid<EntityOrganismDto, OrganismSuggestionQuery> organisms, 
            IEditableGrid<EntityNoteDto> noteGrid) 
            : base(mapper, locationGrid, classificationGrid, organisms, noteGrid)
        {
            this.reactionFacade = reactionFacade;
            textPresenter = new TextPresenter();
        }

        public override void Save()
        {
            var dto = Mapper.Map<BiochemicalReactionDetailDto>(this);
            reactionFacade.Save(dto);
        }

        public void WriteEquation()
        {
            var equationText = textPresenter.ToRawText(Equation);

            Equation = textPresenter.CreateRichText(equationText, reactionFacade.GetClassificationSpans(equationText));
        }

        public void WriteModifier()
        {

        }
    }
}