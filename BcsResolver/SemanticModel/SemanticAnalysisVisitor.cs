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

    public class SemanticAnalisisVisitor : BcsExpressionBuilderVisitor<IBcsBoundSymbol, IBcsBoundSymbol>
    {
        public IBcsWorkspace Workspace { get; }

        public Dictionary<BcsExpressionNode, List<SemanticError>> Errors { get; } = new Dictionary<BcsExpressionNode, List<SemanticError>>();
        public Dictionary<BcsExpressionNode, IBcsBoundSymbol> ResolvedNodeMap { get; } = new Dictionary<BcsExpressionNode, IBcsBoundSymbol>();
        
        public SemanticAnalisisVisitor(IBcsWorkspace workspace)
        {
            Workspace = workspace;
        }

        public override IBcsBoundSymbol Visit(BcsExpressionNode node, IBcsBoundSymbol parameter = null)
        {
            var boundNode = base.Visit(node, parameter);
            ResolvedNodeMap[node] = boundNode;
            return boundNode;
        }

        protected override IBcsBoundSymbol VisitNamedEntitySet(BcsNamedEntitySet bcsNamedEntitySet, IBcsBoundSymbol parameter)
        {
            throw new NotSupportedException("Should be handled by respective entity visits.");
        }

        protected override IBcsBoundSymbol VisitIdentifier(BcsIdentifierNode bcsIdentifier, IBcsBoundSymbol parameter)
        {
            throw new NotSupportedException("Should be handled by respective entity visits.");
        }

        protected override IBcsBoundSymbol VisitNamedReference(BcsNamedEntityReferenceNode bcsNamedEntityReferenceNode, IBcsBoundSymbol parameter)
        {
            var nameToBind = bcsNamedEntityReferenceNode.Identifier?.Name ?? "";
            if (parameter == null)
            {
                BcsLocationSymbol locationSymbol;
                Workspace.Locations.TryGetValue(nameToBind, out locationSymbol);

                if (locationSymbol == null)
                {
                    AddError(bcsNamedEntityReferenceNode, $"Could not find location {nameToBind}.", SemanticErrorSeverity.Error);
                }
                return new BcsBoundLocation() {Syntax = bcsNamedEntityReferenceNode, Symbol = locationSymbol};
            }
            if (parameter is BcsBoundStructuralAgent)
            {
                return new BcsBoundAtomicAgent
                {
                    Syntax = bcsNamedEntityReferenceNode,
                    Symbol = GetSymbolFromParameter<BcsStructuralAgentSymbol>(bcsNamedEntityReferenceNode,parameter,"Structural agent").CastTo<BcsAtomicAgentSymbol>()
                };
            }
            if (parameter is BcsBoundComplex)
            {
                return new BcsBoundStructuralAgent
                {
                    Syntax = bcsNamedEntityReferenceNode,
                    Symbol = GetSymbolFromParameter<BcsComplexSymbol>(bcsNamedEntityReferenceNode, parameter, "Complex").CastTo<BcsStructuralAgentSymbol>()
                };
            }
            if (parameter is BcsBoundLocation)
            {
                var locationSymbol = parameter.CastTo<BcsBoundLocation>().Symbol;

                var complexSymbol = GetSymbolFromWokrspaceByNameAndLocation(nameToBind, locationSymbol, () => Workspace.Complexes);
                if (complexSymbol != null)
                {
                    return new BcsBoundComplex
                    {
                        Syntax = bcsNamedEntityReferenceNode,
                        Symbol = complexSymbol
                    };
                }
                var structuralAgentSymbol = GetSymbolFromWokrspaceByNameAndLocation(nameToBind, locationSymbol, () => Workspace.StructuralAgents);
                if (structuralAgentSymbol != null)
                {
                    return new BcsBoundStructuralAgent()
                    {
                        Syntax = bcsNamedEntityReferenceNode,
                        Symbol = structuralAgentSymbol
                    };
                }
                var atomicAgentSymbol = GetSymbolFromWokrspaceByNameAndLocation(nameToBind, locationSymbol, () => Workspace.AtomicAgents);
                if (atomicAgentSymbol != null)
                {
                    return new BcsBoundAtomicAgent()
                    {
                        Syntax = bcsNamedEntityReferenceNode,
                        Symbol = atomicAgentSymbol
                    };
                }
            }
            return new BcsBoundError
            {
                Syntax = bcsNamedEntityReferenceNode,
                Symbol = new BcsErrorSymbol { Name = nameToBind, Error = "Cannot find such symbol.", ExpectedType = BcsSymbolType.Unknown}
            };
        }

        protected override IBcsBoundSymbol VisitVariableExpression(BcsVariableExpresssioNode bcsVariableExpresssioNode, IBcsBoundSymbol parameter)
        {
            throw new NotImplementedException();
        }

        protected override IBcsBoundSymbol VisitReaction(BcsReactionNode bcsReaction, IBcsBoundSymbol parameter)
        {
            var boundReaction = new BcsBoundReaction
            {
                Syntax = bcsReaction,
                Symbol = null
            };

            foreach (var reactionNode in bcsReaction.LeftSideReactants.Concat(bcsReaction.RightSideReactants))
            {
                var boundContent = Visit(reactionNode);

                var name = boundContent.Symbol.As<BcsNamedSymbol>()?.Name ?? "unnamed";

                boundReaction.AddContent(name, boundContent);
            }
            return boundReaction;
        }

        protected override IBcsBoundSymbol VisitReactant(BcsReactantNode bcsReactant, IBcsBoundSymbol parameter)
        {
            return Visit(bcsReactant.Complex);
        }

        protected override IBcsBoundSymbol VisitComplex(BcsComplexNode bcsComplex, IBcsBoundSymbol parameter)
        {
            var locationSymbol = parameter.As<BcsBoundLocation>()?.Symbol;

            var boundComplex = new BcsBoundComplex
            {
                Syntax = bcsComplex,
                Symbol = GetSymbolByComponentsAndLocation(bcsComplex, locationSymbol)
            };

            BindContent<BcsComplexSymbol, BcsStructuralAgentSymbol>(bcsComplex, boundComplex);

            return boundComplex;
        }

        private BcsComplexSymbol GetSymbolByComponentsAndLocation(BcsComplexNode bcsComplex, BcsLocationSymbol locationSymbol)
        {
            var complexStructuralAgentNames = bcsComplex.Parts.Elements.Select(e => e.Identifier?.Name ?? "").ToList();

            //PERF:
            var candidateComplexes = Workspace.Complexes.Values.Where(
                cx => complexStructuralAgentNames.All(sa => cx.StructuralAgents.Select(ct => ct.Name).Contains(sa)));

            if (locationSymbol != null)
            {
                candidateComplexes =
                    candidateComplexes.Where(cx => cx.Locations.Any(l => l.Name == locationSymbol.Name));
            }
            else
            {
                AddError(bcsComplex, "Location was not provided for complex, looked in the workspace.",
                    SemanticErrorSeverity.Warning);
            }

            var resultComplexes = candidateComplexes.ToList();

            if (resultComplexes.Count > 1)
            {
                AddError(bcsComplex,
                    $"Ambigous reference between complexes: {string.Join(", ", resultComplexes.Select(cx => cx.ToDisplayString()))}",
                    SemanticErrorSeverity.Error);
                return null;
            }
            if (resultComplexes.Count == 0)
            {
                AddError(bcsComplex,
                    $"No complex with such components and location specified: {bcsComplex.ToDisplayString()}",
                    SemanticErrorSeverity.Error);
                return null;
            }

            return resultComplexes.Single();

        }

        protected override IBcsBoundSymbol VisitStructuralAgent(BcsStructuralAgentNode bcsStructuralAgent, IBcsBoundSymbol parameter)
        {
            var boundStructuralAgent = new BcsBoundStructuralAgent
            {
                Syntax = bcsStructuralAgent,
                Symbol =
                    parameter == null
                        ? GetSymbolFromWokrspace(bcsStructuralAgent, () => Workspace.StructuralAgents)
                        : GetSymbolFromParameter<BcsComplexSymbol>(bcsStructuralAgent, parameter, "Structural agent").CastTo<BcsStructuralAgentSymbol>()
            };

            BindContent<BcsStructuralAgentSymbol, BcsAtomicAgentSymbol>(bcsStructuralAgent, boundStructuralAgent);
            return boundStructuralAgent;
        }

        protected override IBcsBoundSymbol VisitAgentState(BcsAgentStateNode bcsAgentState, IBcsBoundSymbol parameter)
        {
            return new BcsBoundAgentState
            {
                Symbol =
                    parameter == null
                        ? null
                        : GetSymbolFromParameter<BcsAtomicAgentSymbol>(bcsAgentState, parameter, "Atomic agent state").CastTo<BcsStateSymbol>(),
                Syntax = bcsAgentState
            };
        }

        protected override IBcsBoundSymbol VisitAtomicAgent(BcsAtomicAgentNode bcsAtomicAgent, IBcsBoundSymbol parameter)
        {
            var boundAtomicAgent = new BcsBoundAtomicAgent()
            {
                Syntax = bcsAtomicAgent,
                Symbol =
                     parameter == null
                         ? GetSymbolFromWokrspace(bcsAtomicAgent, () => Workspace.AtomicAgents)
                         : GetSymbolFromParameter<BcsStructuralAgentSymbol>(bcsAtomicAgent, parameter, "Atomic agent").CastTo<BcsAtomicAgentSymbol>()
            };

            BindContent<BcsAtomicAgentSymbol, BcsStateSymbol>(bcsAtomicAgent, boundAtomicAgent);
            return boundAtomicAgent;
        }

        protected override IBcsBoundSymbol VisitAccessor(BcsContentAccessNode node, IBcsBoundSymbol parameter)
        {
            var boundContainer = Visit(node.Container);

            var directParentSyntax = node.Container.As<BcsContentAccessNode>()?.Child;
            var directBoundParent = directParentSyntax != null ? ResolvedNodeMap[directParentSyntax] : boundContainer;

            

            var boundChild = Visit(node.Child, directBoundParent);

            if (directBoundParent?.Symbol is BcsComposedSymbol)
            {
                var childName = node.Child?.Identifier?.Name ?? "";
                if (string.IsNullOrWhiteSpace(childName))
                {
                    AddError(node, "Entity name expected", SemanticErrorSeverity.Error);
                    return boundContainer;
                }

                if (AreSymbolsCompatibile(node, directBoundParent.Symbol, boundChild.Symbol))
                {
                    directBoundParent.CastTo<IBcsComposedBoundSymbol>().AddContent(childName, boundChild);
                }
            }
            else if (directBoundParent?.Symbol is BcsLocationSymbol)
            {
                var boundLocation = directBoundParent.CastTo<BcsBoundLocation>();

                if (AreSymbolsCompatibile(node, boundLocation.Symbol, boundChild.Symbol))
                {
                    boundLocation.Content = boundChild;
                    return boundLocation;
                }
            }
            else
            {
                AddError(node, $"Type error. Expected composed entity or location. Got '{boundContainer?.Syntax?.ToDisplayString()??""}' instead.", SemanticErrorSeverity.Error);
            }

            return boundContainer;
        }

        private void BindContent<TExpectedParentSymbol, TExpectedChildSymbol>(BcsComposedEntityNode bcsComposedEntityNode, BcsComposedBoundSymbol<TExpectedParentSymbol> containingBoundEntity)
            where TExpectedChildSymbol : BcsNamedSymbol
            where TExpectedParentSymbol : BcsComposedSymbol

        {
            foreach (var element in bcsComposedEntityNode.Parts.Elements)
            {
                var boundContent = Visit(element, containingBoundEntity) as BcsBoundSymbol<TExpectedChildSymbol>;

                if (boundContent?.Symbol == null)
                {
                    AddError(element, $"No entity named {element?.Identifier?.Name??""} found in this context.",SemanticErrorSeverity.Error);
                    continue;
                }

                if (AreSymbolsCompatibile(bcsComposedEntityNode, containingBoundEntity.Symbol, boundContent?.Symbol))
                {
                    containingBoundEntity.AddContent(element.Identifier.Name, boundContent);
                }
            }
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

        private TResultSymbol GetSymbolFromWokrspaceByNameAndLocation<TResultSymbol>(string nameToBind, BcsLocationSymbol locationSymbol, Func<IReadOnlyDictionary<string, TResultSymbol>> symbolProvider )
             where TResultSymbol : BcsComposedSymbol
        {
            TResultSymbol composedSymbol;
            symbolProvider().TryGetValue(nameToBind, out composedSymbol);
            composedSymbol = composedSymbol?.Locations?.Any(l => ReferenceEquals(l, locationSymbol)) == true
                ? composedSymbol
                : null;
            return composedSymbol;
        }

        private BcsNamedSymbol GetSymbolFromParameter<TParentSymbol>(BcsNamedEntityNode namedEntityNode, IBcsBoundSymbol parameter, string resultEntityFriendlyName)
            where TParentSymbol : BcsComposedSymbol
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

        private bool AreSymbolsCompatibile(BcsExpressionNode sourceSyntaxNode, BcsSymbol containerSymbol, BcsSymbol contentSymbol)
        {
            var areCompatible =
                (containerSymbol is BcsComplexSymbol && contentSymbol is BcsStructuralAgentSymbol) ||
                (containerSymbol is BcsStructuralAgentSymbol && contentSymbol is BcsAtomicAgentSymbol) ||
                (containerSymbol is BcsAtomicAgentSymbol && contentSymbol is BcsStateSymbol) ||
                containerSymbol is BcsLocationSymbol && (contentSymbol is BcsComposedSymbol);

            if (!areCompatible)
            {
                AddError(sourceSyntaxNode, $"Type error: {contentSymbol?.Type?? BcsSymbolType.Unknown} is not component of {containerSymbol.Type}", SemanticErrorSeverity.Error);
            }
            return areCompatible;
        }

        private void AddError(BcsExpressionNode identifier, string message, SemanticErrorSeverity severity)
        {
            if (!Errors.ContainsKey(identifier))
            {
                Errors[identifier] = new List<SemanticError>();
            }
            Errors[identifier].Add(new SemanticError(message, severity));
        }

        protected override IBcsBoundSymbol VisitDefault(BcsExpressionNode node, IBcsBoundSymbol parameter)
        {
            throw new NotSupportedException("Should be handled by respective entity visits.");
        }
    }
}
