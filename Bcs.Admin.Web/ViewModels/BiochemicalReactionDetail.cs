using BcsAdmin.BL.Dto;
using System.ComponentModel.DataAnnotations;
using Bcs.Admin.Web.ViewModels.Grids;
using BcsAdmin.BL.Facades;
using AutoMapper;
using BcsAdmin.BL.Queries;
using Bcs.Admin.BL.Dto;

namespace Bcs.Admin.Web.ViewModels
{
    public class BiochemicalReactionDetail : DetailBase
    {
        private readonly ReactionFacade reactionFacade;

        [Display(GroupName = "Fields")]
        public string Equation { get; set; }

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
        }

        public override void Save()
        {
            var dto = Mapper.Map<BiochemicalReactionDetailDto>(this);
            reactionFacade.Save(dto);
        }
    }
}
