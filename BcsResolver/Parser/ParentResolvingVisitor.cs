using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Extensions;

namespace BcsResolver.Parser
{
    public class ParentResolvingVisitor : BcsExpressionNodeVisitor
    {
        protected override void VisitAgent(BcsAtomicAgentNode bcsAtomicAgent)
        {
            ResolveFromParent(bcsAtomicAgent);
        }

        protected override void VisitAgentState(BcsAgentStateNode bcsAgentState)
        {
            ResolveFromParent(bcsAgentState);
        }

        protected override void VisitComplex(BcsComplexNode bcsComplex)
        {
            ResolveFromParent(bcsComplex);
        }

        protected override void VisitComponent(BcsComponentNode bcsComponent)
        {
            ResolveFromParent(bcsComponent);
        }

        protected override void VisitDefault(BcsExpressionNode node)
        {
            ResolveFromParent(node);
        }

        protected override void VisitReactant(BcsReactantNode bcsReactant)
        {
            ResolveFromParent(bcsReactant);
        }

        protected override void VisitReaction(BcsReactionNode bcsReaction)
        {
            ResolveFromParent(bcsReaction);
        }

        protected override void VisitLocation(BcsLocationNode location)
        {
            ResolveFromParent(location);
        }

        private void ResolveFromParent(BcsExpressionNode parentNode)
        {
            foreach (var childNode in parentNode.EnumerateChildNodes())
            {
                childNode.ParentNode = parentNode;
            }
        }
    }
}
