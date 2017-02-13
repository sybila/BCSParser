using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BcsResolver.Syntax.Parser;

namespace BcsAnalysisWeb.ViewModels
{
    public class ReactionViewModel
    {
        public Guid Id { get; set; }
        public string Display { get; set; }

        public BcsReactionNode SyntaxNode { get; set; }
    }
}