using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BcsAdmin.BL.Dto;
using BcsResolver.Extensions;
using BcsResolver.SemanticModel.BoundTree;
using BcsResolver.Syntax.Parser;

namespace BcsAdmin.BL.Facades
{
    public class SemanticColoringVisitor : BoundTreeVisitor
    {
        public List<StyleSpan> SemanticStyleSpans { get; } = new List<StyleSpan>();

        public override void VisitDefault(IBcsBoundSymbol boundSymbol) { }

        protected override void VisitBoundComplex(BcsBoundComplex castTo)
        {
            AddNameRangeStyle(castTo, "complex");
        }  

        protected override void VisitBoundAtomicAgent(BcsBoundAtomicAgent castTo)
        {
            AddNameRangeStyle(castTo, "atomic-agent");
        }

        protected override void VisitBoundStructuralAgent(BcsBoundStructuralAgent castTo)
        {
            AddNameRangeStyle(castTo, "structural-agent");
        }

        protected override void VisitBoundAgentState(BcsBoundAgentState castTo)
        {
            AddNameRangeStyle(castTo, "state");
        }

        protected override void VisitBoundLocation(BcsBoundLocation castTo)
        {
            AddNameRangeStyle(castTo, "location");
        }

        protected override void VisitBoundError(BcsBoundError castTo)
        {
            AddNameRangeStyle(castTo, "error");
        }

        private void AddNameRangeStyle(IBcsBoundSymbol boundSymbol, string cssClass)
        {
            var namedNode = boundSymbol.Syntax.As<BcsNamedEntityNode>();
            if (namedNode?.Identifier != null)
            {
                SemanticStyleSpans.Add(new StyleSpan
                {
                    Range = namedNode.Identifier.NameRange,
                    CssClass = cssClass
                });
            }
        }
    }
}