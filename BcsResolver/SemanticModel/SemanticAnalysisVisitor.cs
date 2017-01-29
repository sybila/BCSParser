using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using BcsResolver.Extensions;
using BcsResolver.File;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Visitors;

namespace BcsResolver.SemanticModel
{

    public class SemantiVisitor : BcsExpressionBuilderVisitor<IBcsBoundSymbol, IBcsBoundSymbol>
    {
        public IBcsWorkspace Workspace { get; }

        public Dictionary<BcsExpressionNode, List<SemanticError>> Errors { get; } = new Dictionary<BcsExpressionNode, List<SemanticError>>();

        public void AddError(BcsExpressionNode identifier, string message, SemanticErrorSeverity severity)
        {
            if (!Errors.ContainsKey(identifier))
            {
                Errors[identifier] = new List<SemanticError>();
            }
            Errors[identifier].Add(new SemanticError(message, severity));
        }

        public SemantiVisitor(IBcsWorkspace workspace)
        {
            Workspace = workspace;
        }
       
        protected override IBcsBoundSymbol VisitNamedEntitySet(BcsNamedEntitySet bcsNamedEntitySet, IBcsBoundSymbol parameter)
        {
            throw new NotImplementedException();
        }

        protected override IBcsBoundSymbol VisitNamedReference(BcsNamedEntityReferenceNode bcsNamedEntityReferenceNode, IBcsBoundSymbol parameter)
        {
            throw new NotImplementedException();
        }

        protected override IBcsBoundSymbol VisitVariableExpression(BcsVariableExpresssioNode bcsVariableExpresssioNode, IBcsBoundSymbol parameter)
        {
            throw new NotImplementedException();
        }

        protected override IBcsBoundSymbol VisitReaction(BcsReactionNode bcsReaction, IBcsBoundSymbol parameter)
        {
            throw new NotImplementedException();
        }

        protected override IBcsBoundSymbol VisitReactant(BcsReactantNode bcsReactant, IBcsBoundSymbol parameter)
        {
            throw new NotImplementedException();
        }

        protected override IBcsBoundSymbol VisitComplex(BcsComplexNode bcsComplex, IBcsBoundSymbol parameter)
        {
            throw new NotImplementedException();
        }

        protected override IBcsBoundSymbol VisitStructuralAgent(BcsStructuralAgentNode bcsStructuralAgent, IBcsBoundSymbol parameter)
        {
            var boundStructuralAgent = new BcsBoundStructuralAgent
            {
                Syntax = bcsStructuralAgent,
                Symbol = 
                    parameter == null 
                        ? GetSymbolFromWokrspace(bcsStructuralAgent, () => Workspace.StructuralAgents) 
                        : GetSymbolFromParameter<BcsStructuralAgentNode, BcsComplexSymbol>(bcsStructuralAgent, parameter, "Structural agent").CastTo<BcsStructuralAgentSymbol>()
            };        

            foreach (var element in bcsStructuralAgent.Parts.Elements)
            {
                var result = Visit(element, boundStructuralAgent) as BcsBoundAtomicAgent;

                boundStructuralAgent.AddContent(element.Identifier.Name, result);
            }
            return boundStructuralAgent;
        }

        protected override IBcsBoundSymbol VisitAgentState(BcsAgentStateNode bcsAgentState, IBcsBoundSymbol parameter)
        {
            var atonicAgent = parameter as BcsBoundAtomicAgent;

            if (atonicAgent == null)
            {
                parameter.Errors.Add(new SemanticError("Atomic agent is not valid in this context.", SemanticErrorSeverity.Error));
                return parameter;
            }

            var stateSymbol =
                atonicAgent.Symbol.States
                    .FirstOrDefault(c => c.Name == bcsAgentState.Identifier.Name);

            return new BcsBoundAgentState
            {
                Symbol = stateSymbol,
                Syntax = bcsAgentState
            };
        }

        protected override IBcsBoundSymbol VisitAtomicAgent(BcsAtomicAgentNode bcsAtomicAgent, IBcsBoundSymbol parameter)
        {
            var structuralAgent = parameter as BcsBoundStructuralAgent;

            if (structuralAgent == null)
            {
                parameter.Errors.Add(new SemanticError("Atomic agent is not valid in this context.", SemanticErrorSeverity.Error));
                return parameter;
            }

            var atomicAgentSymbol =
                structuralAgent.Symbol.AtomicAgents
                    .FirstOrDefault(c => c.Name == bcsAtomicAgent.Identifier.Name);

            var boundStructuralAgent = new BcsBoundAtomicAgent
            {
                Symbol = atomicAgentSymbol,
                Syntax = bcsAtomicAgent,
            };

            if (atomicAgentSymbol == null)
            {
                boundStructuralAgent.Errors.Add(new SemanticError($"Agent: {bcsAtomicAgent.Identifier.Name} does not exist inside: {structuralAgent.Syntax.ToDisplayString()}", SemanticErrorSeverity.Error));
            }

            foreach (var element in bcsAtomicAgent.Parts.Elements)
            {
                var result = Visit(element, boundStructuralAgent) as BcsBoundAgentState;

                boundStructuralAgent.AddContent(element.Identifier.Name, result);
            }
            return boundStructuralAgent;
        }

        protected override IBcsBoundSymbol VisitIdentifier(BcsIdentifierNode identifier, IBcsBoundSymbol parameter)
        {
            throw new NotImplementedException();
        }

        protected override IBcsBoundSymbol VisitDefault(BcsExpressionNode node)
        {
            throw new NotImplementedException();
        }

        protected override IBcsBoundSymbol VisitAccessor(BcsContentAccessNode node, IBcsBoundSymbol parameter)
        {
            throw new NotImplementedException();
        }

        private TResultSymbol GetSymbolFromWokrspace<TSyntaxNode, TResultSymbol>(TSyntaxNode namedEntityNode, Func<IReadOnlyDictionary<string, TResultSymbol>> symbolProvider)
          where TSyntaxNode : BcsNamedEntityNode
          where TResultSymbol : BcsNamedSymbol
        {
            var nameToBind = namedEntityNode.Identifier?.Name ?? "";
            TResultSymbol s;
            symbolProvider().TryGetValue(nameToBind, out s);
            if (s != null)
            {
                AddError(namedEntityNode,
                    $"Location was not provided for entity {nameToBind}, looked in the workspace.",
                    SemanticErrorSeverity.Warning);
            }
            else
            {
                AddError(namedEntityNode, $"Entity {nameToBind}, does not exist in the workspace.",
                    SemanticErrorSeverity.Error);
            }
            return s;
        }

        private BcsNamedSymbol GetSymbolFromParameter<TSyntaxNode, TParentSymbol>(TSyntaxNode namedEntityNode, IBcsBoundSymbol parameter, string resultEntityFriendlyName)
            where TParentSymbol : BcsComposedSymbol
            where TSyntaxNode : BcsNamedEntityNode
        {
            var nameToBind = namedEntityNode.Identifier?.Name ?? "";

            var complex = parameter as BcsComposedBoundSymbol<TParentSymbol>;
            if (complex == null)
            {
                AddError(namedEntityNode, $"{resultEntityFriendlyName} is not valid in this context.", SemanticErrorSeverity.Error);
                return null;
            }

            var resultSymbol =
                complex.Symbol?.Parts?
                    .FirstOrDefault(c => c.Name == namedEntityNode.Identifier.Name);

            if (resultSymbol == null)
            {
                AddError(namedEntityNode, $"{resultEntityFriendlyName}: {nameToBind} does not exist inside: {complex.Syntax.ToDisplayString()}", SemanticErrorSeverity.Error);
            }

            return resultSymbol;
        }


    }
}
