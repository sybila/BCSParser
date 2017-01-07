using System.Linq;

namespace BcsResolver.Syntax.Parser
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
            else if(node is BcsIdentifierNode)
            {
                VisitIdentifier(node as BcsIdentifierNode);
            }
            else if (node is BcsAccessorNode)
            {
                VisitAccessor(node as BcsAccessorNode);
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

        protected abstract void VisitIdentifier(BcsIdentifierNode identifier);

        protected abstract void VisitDefault(BcsExpressionNode node);

        protected abstract void VisitAccessor(BcsAccessorNode node);
    }
}
