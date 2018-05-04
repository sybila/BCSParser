using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Dto;

namespace Bcs.Admin.Web.ViewModels
{
    public class ReactionsTab : TabBase<ReactionRowDto, ReactionFilter>
    {
        public ReactionsTab(ReactionFacade reactionFacade)
            : base(reactionFacade)
        {
            Name = "Reactions";
            Filter = new ReactionFilter();
        }
    }
}

