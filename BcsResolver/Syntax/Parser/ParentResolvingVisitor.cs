namespace BcsResolver.Syntax.Parser
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

        protected override void VisitComponent(BcsStructuralAgentNode bcsStructuralAgent)
        {
            ResolveFromParent(bcsStructuralAgent);
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

        protected override void VisitIdentifier(BcsIdentifierNode identifier)
        {
            ResolveFromParent(identifier);
        }

        private void ResolveFromParent(BcsExpressionNode parentNode)
        {
            foreach (var childNode in parentNode.EnumerateChildNodes())
            {
                childNode.ParentNode = parentNode;
            }
        }

        protected override void VisitAccessor(BcsContentAccessNode node)
        {
            ResolveFromParent(node);
        }
    }
}
