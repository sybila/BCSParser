using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;
using BcsResolver.Extensions;
using BcsResolver.File;
using BcsResolver.SemanticModel.BoundTree;
using BcsResolver.SemanticModel.SymbolTree;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Visitors;

namespace BcsResolver.SemanticModel
{

    public class SemanticAnalisisVisitor : BcsExpressionBuilderVisitor<IBcsBoundSymbol, IBcsBoundSymbol>
    {
        public BcsBoundSymbolFactory BoundSymbolFactory { get; }
        public IBcsWorkspace Workspace { get; }

        public Dictionary<BcsExpressionNode, List<SemanticError>> Errors { get; } = new Dictionary<BcsExpressionNode, List<SemanticError>>();
        public Dictionary<BcsExpressionNode, IBcsBoundSymbol> ResolvedNodeMap { get; } = new Dictionary<BcsExpressionNode, IBcsBoundSymbol>();

        public BcsVariableSymbol DefinedVariable { get; private set; }

        public SemanticAnalisisVisitor(IBcsWorkspace workspace, BcsBoundSymbolFactory boundSymbolFactory)
        {
            Workspace = workspace;
            BoundSymbolFactory = boundSymbolFactory;
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
                BcsLocationSymbol locationSymbol = Workspace.Locations.ContainsKey(nameToBind) ? Workspace.Locations[nameToBind] : null;

                if (locationSymbol == null)
                {
                    AddError(bcsNamedEntityReferenceNode, $"Could not find location {nameToBind}.", SemanticErrorSeverity.Error);
                }
                return BoundSymbolFactory.CreateBoundNamedSymbol(locationSymbol, bcsNamedEntityReferenceNode, BcsSymbolType.Location);
            }
            if (parameter is BcsBoundStructuralAgent)
            {
                var symbol =
                    GetSymbolFromParameter<BcsStructuralAgentSymbol>(
                        bcsNamedEntityReferenceNode,
                        parameter,
                        BcsSymbolType.StructuralAgent);

                return BoundSymbolFactory.CreateBoundNamedSymbol(symbol, bcsNamedEntityReferenceNode, BcsSymbolType.StructuralAgent);
            }
            if (parameter is BcsBoundComplex)
            {
                var symbol =
                    GetSymbolFromParameter<BcsComplexSymbol>(
                        bcsNamedEntityReferenceNode,
                        parameter,
                        BcsSymbolType.Complex);

                return BoundSymbolFactory.CreateBoundNamedSymbol(symbol, bcsNamedEntityReferenceNode, BcsSymbolType.Complex);
            }
            if (parameter is BcsBoundLocation)
            {
                var locationSymbol = parameter.CastTo<BcsBoundLocation>().Symbol;

                var complexSymbol =
                    GetSymbolFromWokrspaceByNameAndLocation(nameToBind, locationSymbol, () => Workspace.Complexes)
                    ?? GetSymbolFromWokrspaceByNameAndLocation(nameToBind, locationSymbol, () => Workspace.StructuralAgents).CastTo<BcsNamedSymbol>()
                    ?? GetSymbolFromWokrspaceByNameAndLocation(nameToBind, locationSymbol, () => Workspace.AtomicAgents);

                if (complexSymbol == null)
                {
                    AddError(bcsNamedEntityReferenceNode, $"Entity '{nameToBind}' does not exist inside {parameter.Syntax.ToDisplayString()}", SemanticErrorSeverity.Error);
                }
                return BoundSymbolFactory.CreateBoundNamedSymbol(complexSymbol, bcsNamedEntityReferenceNode, BcsSymbolType.Unknown);
            }
            return new BcsBoundError
            {
                Syntax = bcsNamedEntityReferenceNode,
                Symbol = new BcsErrorSymbol { Name = nameToBind, Error = "Cannot find such symbol.", ExpectedType = BcsSymbolType.Unknown }
            };
        }

        protected override IBcsBoundSymbol VisitVariableExpression(BcsVariableExpresssioNode bcsVariableExpresssioNode, IBcsBoundSymbol parameter)
        {
            var resolvedSymbols = bcsVariableExpresssioNode.References?.Elements?.Select(e => Visit(e))?.ToList() ?? new List<IBcsBoundSymbol>();
            var firstReferenceSymbol = resolvedSymbols.FirstOrDefault()?.Symbol;


            var variableType = BcsSymbolType.Unknown;
            if (firstReferenceSymbol != null)
            {
                variableType = firstReferenceSymbol.Type;

                resolvedSymbols
                    .Where(rs => rs?.Symbol?.Type != variableType)
                    .ToList()
                    .ForEach(e=> AddError(e.Syntax,$"Entity type missmatch entity is of type {(e?.Symbol.Type ?? BcsSymbolType.Unknown).GetDescription()} while first enity is of type {variableType}.", SemanticErrorSeverity.Error));
            }
            if (string.IsNullOrWhiteSpace(bcsVariableExpresssioNode.VariableName?.Name))
            {
                AddError(bcsVariableExpresssioNode,"Name expected", SemanticErrorSeverity.Error);
            }


            DefinedVariable = new BcsVariableSymbol
            {
                Name = bcsVariableExpresssioNode.VariableName?.Name ?? "",
                AssignedEntities = resolvedSymbols.Select(be => be.Symbol).OfType<BcsNamedSymbol>().ToList(),
                VariableType = variableType
            };

            return new BcsBoundVariableExpression
            {
                Syntax = bcsVariableExpresssioNode,
                Symbol = DefinedVariable,
                Target = Visit(bcsVariableExpresssioNode.TargetExpression)
            };
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

            var boundSymbol = BoundSymbolFactory.CreateBoundNamedSymbol(
                GetSymbolByComponentsAndLocation(bcsComplex, locationSymbol),
                bcsComplex,
                BcsSymbolType.Complex
                );

            BindContent<BcsComplexSymbol, BcsStructuralAgentSymbol>(bcsComplex, boundSymbol);

            return boundSymbol;
        }

        protected override IBcsBoundSymbol VisitStructuralAgent(BcsStructuralAgentNode bcsStructuralAgent, IBcsBoundSymbol parameter)
        {

            var symbol =
                parameter == null
                    ? GetSymbolFromWokrspace(bcsStructuralAgent, () => Workspace.StructuralAgents)
                    : GetSymbolFromParameter<BcsComplexSymbol>(bcsStructuralAgent, parameter, BcsSymbolType.StructuralAgent);

            var boundSymbol = BoundSymbolFactory.CreateBoundNamedSymbol(symbol, bcsStructuralAgent, BcsSymbolType.StructuralAgent);

            BindContent<BcsStructuralAgentSymbol, BcsAtomicAgentSymbol>(bcsStructuralAgent, boundSymbol);

            return boundSymbol;
        }

        protected override IBcsBoundSymbol VisitAgentState(BcsAgentStateNode bcsAgentState, IBcsBoundSymbol parameter)
        {
            var symbol =
                parameter == null
                    ? null
                    : GetSymbolFromParameter<BcsAtomicAgentSymbol>(bcsAgentState, parameter, BcsSymbolType.State);

            return BoundSymbolFactory.CreateBoundNamedSymbol(symbol, bcsAgentState, BcsSymbolType.State);
        }

        protected override IBcsBoundSymbol VisitAtomicAgent(BcsAtomicAgentNode bcsAtomicAgent, IBcsBoundSymbol parameter)
        {
            //Complex can have both atomic and structural agents
            BcsNamedSymbol symbol;
            if (parameter == null)
            {
                symbol = GetSymbolFromWokrspace(bcsAtomicAgent, () => Workspace.AtomicAgents);
            }
            else
            {
                bool isStructuralAgentMissmatch;
                symbol = GetSymbolFromParameter<BcsStructuralAgentSymbol>(bcsAtomicAgent, parameter, BcsSymbolType.Agent, out isStructuralAgentMissmatch);
                if (isStructuralAgentMissmatch)
                {
                    symbol = GetSymbolFromParameter<BcsComplexSymbol>(bcsAtomicAgent, parameter, BcsSymbolType.Agent);
                }
            }

            var boundSymbol = BoundSymbolFactory.CreateBoundNamedSymbol(symbol, bcsAtomicAgent, BcsSymbolType.Agent);

            BindContent<BcsAtomicAgentSymbol, BcsStateSymbol>(bcsAtomicAgent, boundSymbol);

            return boundSymbol;
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
                AddError(node, $"Type error. Expected composed entity or location. Got '{directParentSyntax?.ToDisplayString() ?? ""}' instead.", SemanticErrorSeverity.Error);
            }

            return boundContainer;
        }

        private void BindContent<TExpectedParentSymbol, TExpectedChildSymbol>(BcsComposedEntityNode bcsComposedEntityNode, IBcsBoundSymbol parentBoundSymbol)
            where TExpectedChildSymbol : BcsNamedSymbol
            where TExpectedParentSymbol : BcsComposedSymbol

        {
            var composedBoundEntity = parentBoundSymbol
                .As<BcsComposedBoundSymbol<TExpectedParentSymbol>>();

            if (composedBoundEntity == null) { return; }

            foreach (var element in bcsComposedEntityNode.Parts.Elements)
            {
                var boundContent = Visit(element, composedBoundEntity);

                if (boundContent?.Symbol == null)
                {
                    AddError(element, $"No entity named {element?.Identifier?.Name ?? ""} found in this context.", SemanticErrorSeverity.Error);
                    continue;
                }

                if (AreSymbolsCompatibile(bcsComposedEntityNode, composedBoundEntity.Symbol, boundContent?.Symbol))
                {
                    composedBoundEntity.AddContent(element.Identifier?.Name ?? "", boundContent);
                }
            }
        }

        private BcsComplexSymbol GetSymbolByComponentsAndLocation(BcsComplexNode bcsComplex, BcsLocationSymbol locationSymbol)
        {
            var complexStructuralAgentNames = bcsComplex.Parts.Elements.Select(e => e.Identifier?.Name ?? "").ToList();

            //PERF:
            var candidateComplexes = Workspace.Complexes.Values.Where(
                cx => complexStructuralAgentNames.All(sa => cx.Parts.Select(ct => ct.Name).Contains(sa)));

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

        private TResultSymbol GetSymbolFromWokrspaceByNameAndLocation<TResultSymbol>(string nameToBind, BcsLocationSymbol locationSymbol, Func<IReadOnlyDictionary<string, TResultSymbol>> symbolProvider)
             where TResultSymbol : BcsComposedSymbol
        {
            TResultSymbol composedSymbol;
            symbolProvider().TryGetValue(nameToBind, out composedSymbol);
            composedSymbol = composedSymbol?.Locations?.Any(l => ReferenceEquals(l, locationSymbol)) == true
                ? composedSymbol
                : null;
            return composedSymbol;
        }

        private BcsNamedSymbol GetSymbolFromParameter<TParentSymbol>(BcsNamedEntityNode namedEntityNode, IBcsBoundSymbol parameter, BcsSymbolType expectedType)
            where TParentSymbol : BcsComposedSymbol
        {
            var isNoMatch = false;
            var symbol = GetSymbolFromParameter<TParentSymbol>(namedEntityNode, parameter, expectedType, out isNoMatch);
            AddNoMatchErrorIfAny(isNoMatch, namedEntityNode, expectedType);
            return symbol;
        }

        private BcsNamedSymbol GetSymbolFromParameter<TParentSymbol>(BcsNamedEntityNode namedEntityNode, IBcsBoundSymbol parameter, BcsSymbolType expectedType, out bool noMatch)
            where TParentSymbol : BcsComposedSymbol
        {
            noMatch = false;
            var nameToBind = namedEntityNode.Identifier?.Name ?? "";
            var resultEntityFriendlyName = expectedType.GetDescription();

            var parentBoundSymbol = parameter.As<BcsComposedBoundSymbol<TParentSymbol>>();
            var location = parameter as BcsBoundLocation;

            if (!string.IsNullOrEmpty(location?.Symbol?.Name))
            {
                var symbol = Workspace.LocationEntityMap.GetValue(location?.Symbol?.Name)?
                    .FirstOrDefault(s => s.Type == expectedType && s.Name == nameToBind);

                if (symbol == null)
                {
                    AddError(namedEntityNode,
                        $"{resultEntityFriendlyName} '{nameToBind}' does not exist inside {location.Symbol.Name}",
                        SemanticErrorSeverity.Error);
                }
                return symbol;
            }
            if (parentBoundSymbol != null)
            {
                var resultSymbol =
                    parentBoundSymbol.Symbol?.Parts?
                        .FirstOrDefault(c => c.Name == namedEntityNode.Identifier?.Name);

                if (resultSymbol == null)
                {
                    AddError(namedEntityNode,
                        $"{resultEntityFriendlyName} '{nameToBind}' does not exist inside {parentBoundSymbol.Syntax.ToDisplayString()}",
                        SemanticErrorSeverity.Error);
                }
                return resultSymbol;
            }

            noMatch = true;
            return null;
        }

        private bool AreSymbolsCompatibile(BcsExpressionNode sourceSyntaxNode, BcsSymbol containerSymbol, BcsSymbol contentSymbol)
        {
            var areCompatible =
                containerSymbol.Is<BcsComplexSymbol>() && contentSymbol.Is<BcsStructuralAgentSymbol>() ||
                containerSymbol.Is<BcsComplexSymbol>() && contentSymbol.Is<BcsAtomicAgentSymbol>() ||
                containerSymbol.Is<BcsStructuralAgentSymbol>() && contentSymbol.Is<BcsAtomicAgentSymbol>() ||
                containerSymbol.Is<BcsAtomicAgentSymbol>() && contentSymbol.Is<BcsStateSymbol>() ||
                containerSymbol.Is<BcsLocationSymbol>() && contentSymbol.Is<BcsComposedSymbol>() ||
                containerSymbol.Is<BcsComposedSymbol>() && contentSymbol.Is<BcsErrorSymbol>() ||
                containerSymbol.Is<BcsLocationSymbol>() && contentSymbol.Is<BcsErrorSymbol>();

            if (!areCompatible)
            {
                AddError(sourceSyntaxNode, $"Type error: {contentSymbol?.Type ?? BcsSymbolType.Unknown} is not component of {containerSymbol.Type}", SemanticErrorSeverity.Error);
            }
            return areCompatible;
        }

        private void AddNoMatchErrorIfAny(bool isNoMatch, BcsExpressionNode syntax, BcsSymbolType expectedType)
        {
            if (isNoMatch)
            {
                AddError(syntax, $"{expectedType.GetDescription()} is not valid in this context.",
                    SemanticErrorSeverity.Error);
            }
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
