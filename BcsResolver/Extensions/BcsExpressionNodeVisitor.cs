using BcsResolver.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.Extensions
{
    public abstract class BcsExpressionNodeVisitor
    {
        public void Visit(BcsExpressionNode node)
        {
            if (node is BcsAtomicAgentNode)
            {
                VisitAgent(node as BcsAtomicAgentNode);
            }
            else if (node is BcsAgentStateNode)
            {
                VisitAgentState(node as BcsAgentStateNode);
            }
            else if (node is BcsComponentNode)
            {
                VisitComponent(node as BcsComponentNode);
            }
            else if (node is BcsComplexNode)
            {
                VisitComplex(node as BcsComplexNode);
            }
            else if (node is BcsReactantNode)
            {
                VisitReactant(node as BcsReactantNode);
            }
            else if (node is BcsReactionNode)
            {
                VisitReaction(node as BcsReactionNode);
            }
            else if(node is BcsLocationNode)
            {
                VisitLocation(node as BcsLocationNode);
            }
            else
            {
                VisitDefault(node);
            }

            var children = node.EnumerateChildNodes().ToList();

            foreach (var childNode in children)
            {
                Visit(childNode);
            }
        }

        protected abstract void VisitReaction(BcsReactionNode bcsReaction);
        protected abstract void VisitReactant(BcsReactantNode bcsReactant);

        protected abstract void VisitComplex(BcsComplexNode bcsComplex);

        protected abstract void VisitComponent(BcsComponentNode bcsComponent);

        protected abstract void VisitAgentState(BcsAgentStateNode bcsAgentState);

        protected abstract void VisitAgent(BcsAtomicAgentNode bcsAtomicAgent);

        protected abstract void VisitLocation(BcsLocationNode locationNode);

        protected abstract void VisitDefault(BcsExpressionNode node);
    }
}
