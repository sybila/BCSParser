using System;
using System.Collections.Generic;
using System.Linq;
using BcsResolver.File;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.SemanticModel
{
    class SemanticAnalysisVisitor : BcsExpressionNodeVisitor
    {
        public BcsDefinitionFile DefinitionFile { get; set; }

        protected override void VisitAgent(BcsAtomicAgentNode bcsAtomicAgent)
        { }

        protected override void VisitAgentState(BcsAgentStateNode bcsAgentState)
        {}

        protected override void VisitComponent(BcsStructuralAgentNode bcsStructuralAgent)
        {}

        protected override void VisitComplex(BcsComplexNode bcsComplex)
        { }

        protected override void VisitDefault(BcsExpressionNode node)
        { }

        protected override void VisitReactant(BcsReactantNode bcsReactant)
        { }

        protected override void VisitReaction(BcsReactionNode bcsReaction)
        { }

        protected override void VisitIdentifier(BcsIdentifierNode identifier)
        { }

        protected override void VisitAccessor(BcsContentAccessNode node)
        { }

        private void CheckEntityTreeConsistency<TNode, TParent>(TNode checkedNode, Func<List<TNode>> checkedEntityListGetter, Func<List<TParent>> parentEntityListGetter, Func<TParent, List<TNode>> nodeChildrenGetter)
           where TNode : BcsIdentifierNode
           where TParent : BcsIdentifierNode
        {
            var matchingEntity = checkedEntityListGetter().SingleOrDefault(e => e.Name == checkedNode.Name);

            if (matchingEntity == null)
            {
                checkedNode.Errors.Add(new NodeError($"Undefined entity {checkedNode.Name}."));
                return;
            }

            if (checkedNode.ParentNode is TParent)
            {
                var parentComponent = checkedNode.ParentNode as TParent;
                var matchingComponent = parentEntityListGetter().SingleOrDefault(c => c.Name == parentComponent.Name);

                if (matchingComponent == null)
                {
                    checkedNode.Errors.Add(new NodeError($"Undefined parent entyty {parentComponent.Name}."));
                    return;
                }

                bool isStructureCorrect = nodeChildrenGetter(matchingComponent).Any(a => a.Name == checkedNode.Name);
                if (!isStructureCorrect)
                {
                    checkedNode.Errors.Add(new NodeError($"Entity is not defined in composition of {matchingComponent.Name}"));
                }
            }
            else
            {
                checkedNode.Errors.Add(new NodeError($"Unexpected parent node type."));
            }
        }
    }
}
