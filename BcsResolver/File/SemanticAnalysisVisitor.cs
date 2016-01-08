using BcsResolver.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Parser;

namespace BcsResolver.File
{
    class SemanticAnalysisVisitor : BcsExpressionNodeVisitor
    {
        public BcsDefinitionFile DefinitionFile { get; set; }

        protected override void VisitAgent(BcsAtomicAgentNode bcsAtomicAgent)
        {
            CheckEntityTreeConsistency(
                bcsAtomicAgent,
                () => DefinitionFile.AtomicAgents,
                () => DefinitionFile.Components,
                (component) => component.AtomicAgents
                );

            var matchingAgent = DefinitionFile.AtomicAgents.SingleOrDefault(agent => agent.Name == bcsAtomicAgent.Name);

            if (matchingAgent != null)
            {
                bcsAtomicAgent.AllStates.AddRange(matchingAgent.AllStates.Select(state =>
                new BcsAgentStateNode {
                    UniqueId = (matchingAgent.CurrentState?.Name == state.Name) ? matchingAgent.CurrentState.UniqueId : Guid.NewGuid(),
                    ParentNode = bcsAtomicAgent,
                    Name = state.Name
                }));
            }
        }

        protected override void VisitAgentState(BcsAgentStateNode bcsAgentState)
        {
            CheckEntityTreeConsistency(
                bcsAgentState,
                () => DefinitionFile.States,
                () => DefinitionFile.AtomicAgents,
                (agent) => agent.AllStates
                );
        }

        protected override void VisitComponent(BcsComponentNode bcsComponent)
        {
            //last component last component in :: instane chain: could be location
            if (bcsComponent.ParentNode is BcsComplexNode)
            {
                var parentComplex = bcsComponent.ParentNode as BcsComplexNode;
                var matchingComponent = DefinitionFile.Components.SingleOrDefault(c => c.Name == bcsComponent.Name);
                var matchingLocation = DefinitionFile.Locations.SingleOrDefault(l => l.Name == bcsComponent.Name);

                //OK so by convention ::location has been ommited, 
                //last component in complex, should carry location
                if (matchingComponent != null)
                {
                    var lastComponent = parentComplex.Components.Last();
                    if (!DefinitionFile.Locations.Any(l => l.Name == lastComponent.Name))
                    {
                        bcsComponent.Errors.Add(new NodeError("Locaion information not stated explicitly for component and not stated for last component of complex either."));
                        return;
                    }

                    var newLocation = new BcsLocationNode
                    {
                        Name = lastComponent.Name,
                        Resident = bcsComponent,
                        ParentNode = parentComplex
                    };

                    bcsComponent.ParentNode = newLocation;

                    parentComplex.Components.Remove(bcsComponent);
                    parentComplex.Components.Add(newLocation);
                }
                //so this is location, not component, fix the tree
                else if (matchingLocation != null)
                {
                    //in reaction or modifier context there should not be more than one
                    var childComponent = bcsComponent.SubComponents.FirstOrDefault();

                    var newLocation = new BcsLocationNode
                    {
                        Name = matchingLocation.Name,
                        Resident = childComponent,
                        ParentNode = parentComplex
                    };

                    if (childComponent == null)
                    {
                        newLocation.Errors.Add(new NodeError("Location stated without component following."));
                    }

                    childComponent.ParentNode = newLocation;

                    parentComplex.Components.Remove(bcsComponent);
                    parentComplex.Components.Add(newLocation);
                }
            }
            //definitely not location
            else if (bcsComponent.ParentNode is BcsComponentNode)
            {
                //must be defined among complexes
                CheckEntityTreeConsistency(
                bcsComponent,
                () => DefinitionFile.Components,
                () => DefinitionFile.Complexes,
                (complex) => complex.Components.Where(c => c is BcsComponentNode).Cast<BcsComponentNode>().ToList()
                );
            }
        }

        protected override void VisitComplex(BcsComplexNode bcsComplex)
        { }

        protected override void VisitDefault(BcsExpressionNode node)
        { }

        protected override void VisitReactant(BcsReactantNode bcsReactant)
        { }

        protected override void VisitReaction(BcsReactionNode bcsReaction)
        { }

        protected override void VisitLocation(BcsLocationNode locationNode)
        { }

        private void CheckEntityTreeConsistency<TNode, TParent>(TNode checkedNode, Func<List<TNode>> checkedEntityListGetter, Func<List<TParent>> parentEntityListGetter, Func<TParent, List<TNode>> nodeChildrenGetter)
           where TNode : BcsEntityNode
           where TParent : BcsEntityNode
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
