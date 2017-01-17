using System;
using System.Collections.Generic;
using System.Linq;
using BcsResolver.Extensions;
using BcsResolver.Syntax.Tokenizer;
using Microsoft.Build.Utilities;

namespace BcsResolver.Syntax.Parser
{
    public class BcsParser : ParserBase<BcsExpresionToken, BcsExpresionTokenType>
    {
        protected override BcsExpresionTokenType WhiteSpaceToken => BcsExpresionTokenType.Whitespace;

        public BcsReactionNode ParseReaction(List<BcsExpresionToken> tokens)
        {
            if (tokens.Count < 1) { return null; }
            //whitespace tokens are never  significant, leave then out.
            Tokens = tokens.Where(t => t.Type != BcsExpresionTokenType.Whitespace).ToList();
            CurrentIndex = 0;

            var reaction = ReadReaction();

            PostProcessTree(reaction);

            return reaction;
        }

        public BcsExpressionNode ParseComplex(List<BcsExpresionToken> tokens)
        {
            if (tokens.Count < 1) { return null; }
            //whitespace tokens are never  significant, leave then out.
            Tokens = tokens.Where(t => t.Type != BcsExpresionTokenType.Whitespace).ToList();
            CurrentIndex = 0;

            return ReadComponentAccess();
        }

        private static void PostProcessTree(BcsReactionNode reaction)
        {
            var visitor = new ParentResolvingVisitor();
            visitor.Visit(reaction);
        }

        private BcsReactionNode ReadReaction()
        {
            var reaction = new BcsReactionNode();

            ReadReactionLeftSide(reaction);

            ReadReactionDirection(reaction);

            ReadReactionRightSide(reaction);
            return reaction;
        }

        private void ReadReactionDirection(BcsReactionNode reaction)
        {
            if (IsPeekReactionDirection())
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
            while (Peek() != null && !IsPeekReactionDirection())
            {
                reaction.LeftSideReactants.Add(ReadReactant());

                if (IsPeekType(BcsExpresionTokenType.Interaction))
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

                if (IsPeekType(BcsExpresionTokenType.Interaction))
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

            reactant.Complex = ReadComponentAccess();

            return reactant;
        }

        private BcsExpressionNode ReadComponentAccess()
        {
            BcsExpressionNode target = ReadComplex();

            while (IsPeekType(BcsExpresionTokenType.FourDot))
            {
                target = new BcsContentAccessNode
                {
                    Operator = Read().ToTextRange(),
                    Target = target,
                    Container = ReadComplex()
                };
            }
            return target;
        }

        private BcsNamedEntityNode ReadComplex()
        {
            var component = ReadComponentOrAgentOrReference();

            if (IsPeekType(BcsExpresionTokenType.Dot))
            {
                var complex = new BcsComplexNode { Identifier = null };
                complex.Parts.Add(component);

                while (IsPeekType(BcsExpresionTokenType.Dot))
                {
                    complex.Separators.Add(Read().ToTextRange());
                    complex.Parts.Add(ReadComponent());
                }
                return complex;
            }
            return component;
        }

        private BcsNamedEntityNode ReadComponentOrAgentOrReference()
        {
            var identifier = ReadIdentifier();

            if (IsPeekType(BcsExpresionTokenType.ComponentBegin))
            {
                return ReadComponent(identifier);
            }
            if (IsPeekType(BcsExpresionTokenType.AgentBegin))
            {
                return ReadAtomicAgent(identifier);
            }
            return new BcsNamedEntityReferenceNode { Identifier = identifier }; ;
        }

        private BcsExpressionNode ReadComponent()
        {
            var identifier = ReadIdentifier();
            return ReadComponent(identifier);
        }

        private BcsNamedEntityNode ReadComponent(BcsIdentifierNode identifier)
        {
            if (IsPeekType(BcsExpresionTokenType.ComponentBegin))
            {
                var component = new BcsStructuralAgentNode { Identifier = identifier };
                component.BeginBrace = Read().ToTextRange();

                //case B()
                if (IsPeekType(BcsExpresionTokenType.Identifier))
                {
                    //case B(I)
                    component.Parts.Add(ReadAtomicAgent());
                }
                while (IsPeekType(BcsExpresionTokenType.Comma))
                {
                    //case B(I,I,...,I)
                    component.Separators.Add(Read().ToTextRange());
                    component.Parts.Add(ReadAtomicAgent());
                }
                component.EndBrace =
                    CheckedRead(BcsExpresionTokenType.ComponentEnd, component)
                    .ToTextRange();

                return component;
            }
            return new BcsNamedEntityReferenceNode { Identifier = identifier };
        }

        private BcsExpressionNode ReadAtomicAgent()
        {
            var identifier = ReadIdentifier();
            return ReadAtomicAgent(identifier);
        }

        private BcsNamedEntityNode ReadAtomicAgent(BcsIdentifierNode identifier)
        {
            if (IsPeekType(BcsExpresionTokenType.AgentBegin))
            {
                var agent = new BcsAtomicAgentNode { Identifier = identifier };
                agent.BeginBrace = Read().ToTextRange();
                agent.Parts.Add(ReadState());

                while (IsPeekType(BcsExpresionTokenType.Comma))
                {
                    agent.Separators.Add(Read().ToTextRange());
                    agent.Parts.Add(ReadState());
                }

                agent.EndBrace =
                    CheckedRead(BcsExpresionTokenType.AgentEnd, agent)
                    .ToTextRange();

                return agent;
            }
            return new BcsNamedEntityReferenceNode { Identifier = identifier };
        }

        private BcsAgentStateNode ReadState()
        {
            return new BcsAgentStateNode { Identifier = ReadIdentifier() };
        }

        private BcsIdentifierNode ReadIdentifier()
        {
            var identifier = new BcsIdentifierNode { };
            var wsBefore = SkipWhiteSpace();
            var identifierToken = CheckedRead(BcsExpresionTokenType.Identifier, identifier);
            var wsAfter = SkipWhiteSpace();

            identifier.Name = identifierToken.Text;
            identifier.NameRange = identifierToken.ToTextRange();
            identifier.WhiteSpacesBefore.AddRange(wsBefore.Select(t => t.ToTextRange()));
            identifier.WhiteSpacesAfter.AddRange(wsAfter.Select(t => t.ToTextRange()));
            return identifier;
        }

        private void ReadCoeficient(BcsReactantNode reactant)
        {
            if (IsPeekType(BcsExpresionTokenType.ReactionCoeficient))
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
            if (IsPeekType(type))
            {
                return Read();
            }
            else
            {
                expression.Errors.Add(new NodeError("Unexpected token type.", Peek().ToTextRange(), Peek()));
                if (throwException)
                {
                    throw new ParserException($"Unexpected token type: {Peek()?.Type} index: {CurrentIndex}");
                }
            }
            return null;
        }

        private bool IsPeekType(BcsExpresionTokenType type) => Peek() != null && Peek().Type == type;

        private bool IsPeekReactionDirection()
        {
            if (Peek() == null) return false;
            var type = Peek().Type;
            return
                type == BcsExpresionTokenType.ReactionDirectionBoth
                || type == BcsExpresionTokenType.ReactionDirectionLeft
                || type == BcsExpresionTokenType.ReactionDirectionRight;
        }
    }
}
