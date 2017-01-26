using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using BcsResolver.Extensions;
using BcsResolver.Syntax.Tokenizer;
using Microsoft.Build.Utilities;

namespace BcsResolver.Syntax.Parser
{
    public class BcsParser : ParserBase<BcsExpresionToken, BcsExpresionTokenType>
    {
        protected override BcsExpresionTokenType WhiteSpaceToken => BcsExpresionTokenType.Whitespace;

        public BcsExpressionNode ParseReaction(List<BcsExpresionToken> tokens)
        {
            if (tokens.Count < 1)
            {
                return null;
            }
            //whitespace tokens are never  significant, leave then out.
            Tokens = tokens.Where(t => t.Type != BcsExpresionTokenType.Whitespace).ToList();
            CurrentIndex = 0;

            var reaction = ParseVariableDefinition();

            PostProcessTree(reaction);

            return reaction;
        }

        public BcsExpressionNode ParseComplex(List<BcsExpresionToken> tokens)
        {
            if (tokens.Count < 1)
            {
                return null;
            }
            //whitespace tokens are never  significant, leave then out.
            Tokens = tokens.Where(t => t.Type != BcsExpresionTokenType.Whitespace).ToList();
            CurrentIndex = 0;

            return ReadComponentAccess();
        }

        private static void PostProcessTree(BcsExpressionNode reaction)
        {
            var visitor = new ParentResolvingVisitor();
            visitor.Visit(reaction);
        }

        private BcsExpressionNode ParseVariableDefinition()
        {
            var reaction = ReadReaction();
            if (IsPeekType(BcsExpresionTokenType.Semicolon))
            {
                var variableExpression = new BcsVariableExpresssioNode
                {
                    DefinitionSeparator = Read().ToTextRange(),
                    TargetExpression = reaction
                };

                variableExpression.VariableName = ReadIdentifier();
                //TODO: Add Error if not

                if (IsPeekType(BcsExpresionTokenType.Assignment))
                {
                    variableExpression.AssignmentOperator = Read().ToTextRange();

                    var set = ReadSet(
                        BcsExpresionTokenType.Comma,
                        ReadComplex,
                        BcsExpresionTokenType.SetBegin,
                        BcsExpresionTokenType.SetEnd);
                    variableExpression.References = set;
                }
                else
                {
                    variableExpression.Errors.Add(new NodeError("Expected assigmmennt of variable.",
                        Peek()?.ToTextRange() ?? default(TextRange)));
                }
                return variableExpression;
            }
            return reaction;
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
            SafeLoop(
                () => Peek() != null
                && !IsPeekReactionDirection(),
                () =>
                {
                    reaction.LeftSideReactants.Add(ReadReactant());

                    if (IsPeekType(BcsExpresionTokenType.Interaction))
                    {
                        reaction.InteractionSeparatorRanges.Add(Peek().ToTextRange());
                        Read();
                    }
                });
        }

        private void ReadReactionRightSide(BcsReactionNode reaction)
        {
            SafeLoop(
                () => IsPeekType(BcsExpresionTokenType.Identifier)
                      || IsPeekType(BcsExpresionTokenType.ReactionCoeficient)
                      || IsPeekType(BcsExpresionTokenType.QuestionMark),
                () =>
                {
                    reaction.RightSideReactants.Add(ReadReactant());

                    if (IsPeekType(BcsExpresionTokenType.Interaction))
                    {
                        reaction.InteractionSeparatorRanges.Add(Peek().ToTextRange());
                        Read();
                    }
                });
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

            SafeLoop(
                () => IsPeekType(BcsExpresionTokenType.FourDot),
                () =>
                {
                    target = new BcsContentAccessNode
                    {
                        Operator = Read().ToTextRange(),
                        Target = target,
                        Container = ReadComplex()
                    };
                });
            return target;
        }

        private BcsNamedEntityNode ReadComplex()
        {
            var component = ReadComponentOrAgentOrReference();

            if (IsPeekType(BcsExpresionTokenType.Dot))
            {
                var complex = new BcsComplexNode
                {
                    Identifier = null,
                    Parts = ReadSet(BcsExpresionTokenType.Dot, ReadComponent, firstElement: component)
                };
                return complex;
            }
            return component;
        }

        private BcsNamedEntityNode ReadComponentOrAgentOrReference()
        {
            return ReadVariableOrEntity(() =>
            {
                var identifier = ReadIdentifier();
                if (IsPeekType(BcsExpresionTokenType.BracketBegin))
                {
                    return ReadComponent(identifier);
                }
                if (IsPeekType(BcsExpresionTokenType.SetBegin))
                {
                    return ReadAtomicAgent(identifier);
                }
                return new BcsNamedEntityReferenceNode { Identifier = identifier };
            });
        }

        private BcsNamedEntityNode ReadComponent()
        {
            return ReadVariableOrEntity(() =>
            {
                var identifier = ReadIdentifier();
                return ReadComponent(identifier);
            });
        }

        private BcsNamedEntityNode ReadComponent(BcsIdentifierNode identifier)
        {
            if (IsPeekType(BcsExpresionTokenType.BracketBegin))
            {
                var component = new BcsStructuralAgentNode
                {
                    Identifier = identifier,
                    Parts = ReadSet(
                        BcsExpresionTokenType.Comma,
                        ReadAtomicAgent, BcsExpresionTokenType.BracketBegin,
                        BcsExpresionTokenType.BracketEnd,
                        allowEmpty: true)
                };

                return component;
            }
            return new BcsNamedEntityReferenceNode { Identifier = identifier };
        }

        private BcsNamedEntityNode ReadAtomicAgent()
        {
            return ReadVariableOrEntity(() =>
            {
                var identifier = ReadIdentifier();
                return ReadAtomicAgent(identifier);
            });
        }

        private BcsNamedEntityNode ReadAtomicAgent(BcsIdentifierNode identifier)
        {
            if (IsPeekType(BcsExpresionTokenType.SetBegin))
            {
                var agent = new BcsAtomicAgentNode
                {
                    Identifier = identifier,
                    Parts = ReadSet(BcsExpresionTokenType.Comma, ReadState, BcsExpresionTokenType.SetBegin, BcsExpresionTokenType.SetEnd, allowEmpty: true)
                };

                return agent;
            }
            return new BcsNamedEntityReferenceNode { Identifier = identifier };
        }

        private BcsNamedEntityNode ReadState()
        {
            return ReadVariableOrEntity(() => new BcsAgentStateNode { Identifier = ReadIdentifier() });
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

        private BcsIdentifierNode ReadIdentifier()
        {
            var identifier = new BcsIdentifierNode { };
            var identifierToken = CheckedRead(BcsExpresionTokenType.Identifier, identifier.Errors);
            if (identifierToken == null)
            {
                return null;
            }

            identifier.Name = identifierToken?.Text;
            identifier.NameRange = identifierToken?.ToTextRange() ?? default(TextRange);
            return identifier;
        }

        private BcsNamedEntityNode ReadVariableOrEntity<TEntity>(Func<TEntity> entityReadFunc)
            where TEntity : BcsNamedEntityNode
        {
            if (IsPeekType(BcsExpresionTokenType.QuestionMark))
            {
                return new BcsNamedEntityReferenceNode
                {
                    QuestionMark = Read(),
                    Identifier = ReadIdentifier()
                };
            }

            return entityReadFunc();
        }

        private BcsNamedEntitySet ReadSet(
            BcsExpresionTokenType separatorTokenType,
            Func<BcsNamedEntityNode> elementReadFunc,
            BcsExpresionTokenType? openingTokenType = null,
            BcsExpresionTokenType? closingTokenType = null,
            BcsNamedEntityNode firstElement = null,
            bool allowEmpty = false)
        {
            var set = new BcsNamedEntitySet();

            if (openingTokenType.HasValue)
            {
                set.OpeningToken = CheckedRead(openingTokenType.Value, set.Errors);
                if (set.OpeningToken == null)
                {
                    return set;
                }
            }

            if (IsPeekType(BcsExpresionTokenType.Identifier) || IsPeekType(BcsExpresionTokenType.QuestionMark))
            {
                firstElement = firstElement ?? elementReadFunc();
            }

            if (!allowEmpty && firstElement == null)
            {
                set.Errors.Add(
                    new NodeError("Element identifier expected.",
                        Peek()?.ToTextRange()
                        ?? new TextRange(Tokens.LastOrDefault()?.StartPosition ?? 0, 0)));
            }
            set.Elements.Add(firstElement);

            SafeLoop(() => IsPeekType(separatorTokenType), () =>
            {
                set.SeparatorTokens.Add(Read());
                set.Elements.Add(elementReadFunc());
            });

            if (closingTokenType.HasValue)
            {
                set.ClosingToken = CheckedRead(closingTokenType.Value, set.Errors);
            }
            return set;
        }

        private void SafeLoop(Func<bool> loopCondition, Action loopAction)
        {
            var lastLoopIndex = CurrentIndex;
            while (loopCondition())
            {
                loopAction();

                if (lastLoopIndex >= CurrentIndex)
                {
                    break;
                }
                lastLoopIndex = CurrentIndex;
            }
        }

        private BcsExpresionToken CheckedRead(BcsExpresionTokenType type, List<NodeError> errors,
            bool throwException = false)
        {
            if (IsPeekType(type))
            {
                return Read();
            }
            else
            {
                errors.Add(new NodeError("Unexpected token type.", Peek().ToTextRange(), Peek()));
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
