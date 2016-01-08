using BcsResolver.Extensions;
using BcsResolver.Tokenizer;
using DotVVM.Framework.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.Parser
{
    public class BcsParser : ParserBase<BcsExpresionToken, BcsExpresionTokenType>
    {
        protected override BcsExpresionTokenType WhiteSpaceToken => BcsExpresionTokenType.Whitespace;

        private Random randomizer = new Random();

        public BcsReactionNode ParseReaction(List<BcsExpresionToken> tokens)
        {
            if (tokens.Count < 1) { return null; }
            //whitespace tokens are never  significant, leave then out.
            Tokens = tokens.Where(t => t.Type != BcsExpresionTokenType.Whitespace).ToList();
            CurrentIndex = 0;

            var reaction = new BcsReactionNode();

            ReadReactionLeftSide(reaction);

            ReadReactionDirection(reaction);

            ReadReactionRightSide(reaction);

            PostProcessTree(reaction);

            return reaction;
        }

        public BcsComplexNode ParseComplex(List<BcsExpresionToken> tokens)
        {
            if (tokens.Count < 1) { return null; }
            //whitespace tokens are never  significant, leave then out.
            Tokens = tokens.Where(t => t.Type != BcsExpresionTokenType.Whitespace).ToList();
            CurrentIndex = 0;

            return ReadComplex();
        }

        private static void PostProcessTree(BcsReactionNode reaction)
        {
            var visitor = new ParentResolvingVisitor();
            visitor.Visit(reaction);
        }

        private void ReadReactionDirection(BcsReactionNode reaction)
        {
            if (IsReactionDirection(Peek().Type))
            {
                var directionToken = Read();

                switch (directionToken.Type)
                {
                    case BcsExpresionTokenType.ReactionDirectionLeft:
                        reaction.ReactionDirection = ReactionDirectionType.Left;
                        break;
                    case BcsExpresionTokenType.ReactionDirectionRight:
                        reaction.ReactionDirection = ReactionDirectionType.Right;
                        break;
                    case BcsExpresionTokenType.ReactionDirectionBoth:
                        reaction.ReactionDirection = ReactionDirectionType.Both;
                        break;
                }
                reaction.ReactionDirectionRange = directionToken.ToTextRange();
            }
        }

        private void ReadReactionLeftSide(BcsReactionNode reaction)
        {
            while (Peek() != null && !IsReactionDirection(Peek().Type))
            {
                reaction.LeftSideReactants.Add(ReadReactant());

                if (Peek().Type == BcsExpresionTokenType.AgentInteractionPositive)
                {
                    reaction.InteractionSeparatorRanges.Add(Peek().ToTextRange());
                    Read();
                }
            }
        }

        private void ReadReactionRightSide(BcsReactionNode reaction)
        {
            while (Peek() != null)
            {
                reaction.RightSideReactants.Add(ReadReactant());

                if (Peek() != null && Peek().Type == BcsExpresionTokenType.AgentInteractionPositive)
                {
                    reaction.InteractionSeparatorRanges.Add(Peek().ToTextRange());
                    Read();
                }
            }
        }

        private BcsReactantNode ReadReactant()
        {
            BcsReactantNode reactant = new BcsReactantNode();

            ReadCoeficient(reactant);

            reactant.Complex = ReadComplex();

            return reactant;
        }

        private BcsComplexNode ReadComplex()
        {
            var complex = new BcsComplexNode() { Name = $"__complex{randomizer.Next() % 10000}"};

            CheckedRead(BcsExpresionTokenType.ComplexStart, complex);

            complex.Components.Add(ReadComponentInstance(complex));

            while (Peek().Type == BcsExpresionTokenType.CompositionSeparator)
            {
                complex.Separators.Add(Peek().ToTextRange());
                Read();

                complex.Components.Add(ReadComponentInstance(complex));
            }

            CheckedRead(BcsExpresionTokenType.ComplexEnd, complex);
            return complex;
        }

        private BcsComponentNode ReadComponentInstance(BcsComplexNode complex)
        {
            var entityStack = new Stack<BcsEntityNode>();

            CheckedRead(BcsExpresionTokenType.ComponentStart, complex);
            var identifierToken = CheckedRead(BcsExpresionTokenType.AgentIdentifier, complex);

            if (Peek().Type == BcsExpresionTokenType.AgentStateBegin)
            {
                entityStack.Push(ReadAtomicAgent(identifierToken));
            }
            else if (Peek().Type == BcsExpresionTokenType.CompositionBegin)
            {
                entityStack.Push(ReadComponent(identifierToken));
            }
            //just name, no composition there
            else if (Peek().Type == BcsExpresionTokenType.ComponentEnd)
            {
                entityStack.Push(new BcsComponentNode { Name = identifierToken.Text, NameRange = identifierToken.ToTextRange() });
            }

            CheckedRead(BcsExpresionTokenType.ComponentEnd, complex);

            while (Peek().Type == BcsExpresionTokenType.InheritanceMark)
            {
                //read subkomponent instanciated by ::
                Read();

                CheckedRead(BcsExpresionTokenType.ComponentStart, complex);
                identifierToken = CheckedRead(BcsExpresionTokenType.AgentIdentifier, complex);
                entityStack.Push(ReadComponent(identifierToken));
                CheckedRead(BcsExpresionTokenType.ComponentEnd, complex);
            }

            //construct component hierarchy
            return CreateComponentInstanceHierarchy(complex, entityStack);
        }

        private static BcsComponentNode CreateComponentInstanceHierarchy(BcsComplexNode complex, Stack<BcsEntityNode> entityStack)
        {
            bool atomicAgentFound = false;
            BcsComponentNode parentComponent = null;
            BcsComponentNode rootComponent = null;

            while (entityStack.Count != 0)
            {
                if (atomicAgentFound)
                {
                    complex.Errors.Add(new NodeError("Atomic agent must be listed last in complex hierarchy."));
                    break;
                }

                var currentEntity = entityStack.Pop();

                if (currentEntity is BcsAtomicAgentNode)
                {
                    atomicAgentFound = true;

                    if (parentComponent == null)
                    {
                        complex.Errors.Add(new NodeError("Complex cannot directly contain atomic agent."));
                        break;
                    }

                    parentComponent.AtomicAgents.Add(currentEntity as BcsAtomicAgentNode);
                }
                else if (currentEntity is BcsComponentNode)
                {
                    if (parentComponent != null)
                    {
                        var componentChild = currentEntity as BcsComponentNode;
                        parentComponent.SubComponents.Add(componentChild);
                        parentComponent = componentChild;
                    }
                    else
                    {
                        parentComponent = currentEntity as BcsComponentNode;
                        rootComponent = parentComponent;
                    }
                    
                }
            }

            return rootComponent;
        }

        private BcsComponentNode ReadComponent(BcsExpresionToken identifierToken)
        {
            var component = new BcsComponentNode();
            component.Name = identifierToken.Text;
            component.NameRange = identifierToken.ToTextRange();

            if (Peek().Type == BcsExpresionTokenType.CompositionBegin)
            {
                Read();

                if (Peek().Type != BcsExpresionTokenType.CompositionEnd)
                {
                    var agentIdentifierToken = CheckedRead(BcsExpresionTokenType.AgentIdentifier, component);
                    component.AtomicAgents.Add(ReadAtomicAgent(agentIdentifierToken));

                    while (Peek().Type == BcsExpresionTokenType.AgentSeparator)
                    {
                        Read();

                        agentIdentifierToken = CheckedRead(BcsExpresionTokenType.AgentIdentifier, component);
                        component.AtomicAgents.Add(ReadAtomicAgent(agentIdentifierToken));
                    }


                }

                CheckedRead(BcsExpresionTokenType.CompositionEnd, component);
            }

            return component;
        }

        private BcsAtomicAgentNode ReadAtomicAgent(BcsExpresionToken identifierToken)
        {
            var beginRange = Peek().ToTextRange();
            Read();

            var atommicAgent = new BcsAtomicAgentNode();
            var stateToken = CheckedRead(BcsExpresionTokenType.AgentStateIdentifier, atommicAgent);

            atommicAgent.Name = identifierToken.Text;
            atommicAgent.NameRange = identifierToken.ToTextRange();
            atommicAgent.StateBeginRange = beginRange;
            atommicAgent.CurrentState = new BcsAgentStateNode() { Name = stateToken.Text, NameRange = stateToken.ToTextRange() };

            var endToken = CheckedRead(BcsExpresionTokenType.AgentStateEnd, atommicAgent);

            if (endToken != null) { atommicAgent.StateEndRange = endToken.ToTextRange(); }

            return atommicAgent;
        }

        private void ReadCoeficient(BcsReactantNode reactant)
        {
            if (Peek().Type == BcsExpresionTokenType.ReactionCoeficient)
            {
                double coeficient = 0;
                if (double.TryParse(Peek().Text, out coeficient))
                {
                    reactant.Coeficient = coeficient;
                }
                else
                {
                    reactant.Errors.Add(new NodeError("Coeficient has invalid number format.", Peek().ToTextRange()));
                }
                reactant.CoeficientRange = Peek().ToTextRange();
                Read();
            }
        }

        private BcsExpresionToken CheckedRead(BcsExpresionTokenType type, BcsExpressionNode expression, bool throwException = false)
        {
            if (Peek().Type == type)
            {
                return Read();
            }
            else
            {
                expression.Errors.Add(new NodeError("Unexpected token type.", Peek().ToTextRange(), Peek()));
                if (throwException)
                {
                    throw new ParserException($"Unexpected token type: {Peek().Type} index: {CurrentIndex}");
                }
            }
            return null;
        }

        private bool IsReactionDirection(BcsExpresionTokenType type)
        {
            return type == BcsExpresionTokenType.ReactionDirectionBoth || type == BcsExpresionTokenType.ReactionDirectionLeft || type == BcsExpresionTokenType.ReactionDirectionRight;
        }
    }
}
