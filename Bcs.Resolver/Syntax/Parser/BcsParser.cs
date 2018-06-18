using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using BcsResolver.Extensions;
using BcsResolver.Syntax.Tokenizer;
using BcsResolver.Syntax.Visitors;

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
            var startChar = LastChar;
            var reaction = ReadReaction();
            if (IsPeekType(BcsExpresionTokenType.Semicolon))
            {
                var variableExpression = new BcsVariableExpresssioNode
                {
                    DefinitionSeparator = Read().ToTextRange(),
                    TargetExpression = reaction
                };

                SkipWhiteSpace();

                if (IsPeekType(BcsExpresionTokenType.QuestionMark))
                {
                    Read();
                }

                var identifier = ReadIdentifier();
                variableExpression.VariableName = identifier;
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
                variableExpression.ExpressioRange = TextRange.FromBounds(startChar, LastChar);
                return variableExpression;
            }
            return reaction;
        }


        private BcsReactionNode ReadReaction()
        {
            var startChar = LastChar;
            var reaction = new BcsReactionNode();

            ReadReactionLeftSide(reaction);

            ReadReactionDirection(reaction);

            ReadReactionRightSide(reaction);
            reaction.ExpressioRange = TextRange.FromBounds(startChar, LastChar);
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

                    if (IsPeekPlusToken())
                    {
                        reaction.InteractionSeparatorRanges.Add(Peek().ToTextRange());
                        Read();
                    }
                });
        }

        private bool IsPeekPlusToken()
        {
            return IsPeekType(BcsExpresionTokenType.Identifier) && Peek().Text == "+";
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

                    if (IsPeekPlusToken())
                    {
                        reaction.InteractionSeparatorRanges.Add(Peek().ToTextRange());
                        Read();
                    }
                });
        }

        private BcsReactantNode ReadReactant()
        {
            var startChar = LastChar;
            BcsReactantNode reactant = new BcsReactantNode();

            ReadCoeficient(reactant);

            reactant.Complex = ReadComponentAccess();
            reactant.ExpressioRange = TextRange.FromBounds(startChar, LastChar);

            return reactant;
        }

        private BcsExpressionNode ReadComponentAccess()
        {
            var startChar = LastChar;
            BcsNamedEntityNode child = ReadComplex();

            if (IsPeekType(BcsExpresionTokenType.FourDot))
            {
                return new BcsContentAccessNode
                {
                    Operator = Read().ToTextRange(),
                    Child = child,
                    Container = ReadComponentAccess(),
                    ExpressioRange = TextRange.FromBounds(startChar, LastChar)
                };

            }
            return child;
        }

        private BcsNamedEntityNode ReadComplex()
        {
            var startChar = LastChar;
            var component = ReadComponentOrAgentOrReference().CheckExpectedIdentifierError();

            if (IsPeekType(BcsExpresionTokenType.Dot))
            {
                var complex = new BcsComplexNode
                {
                    Identifier = null,
                    Parts = ReadSet(BcsExpresionTokenType.Dot, ReadComponentOrAgentOrReference, firstElement: component),
                    ExpressioRange = TextRange.FromBounds(startChar, LastChar)
                };
                return complex;
            }
            return component;
        }

        private BcsNamedEntityNode ReadComponentOrAgentOrReference()
        {
            return ReadVariableOrEntity(() =>
            {
                var startChar = LastChar;
                var identifier = ReadIdentifier();
                if (IsPeekType(BcsExpresionTokenType.BracketBegin))
                {
                    return ReadComponent(identifier, startChar);
                }
                if (IsPeekType(BcsExpresionTokenType.SetBegin))
                {
                    return ReadAtomicAgent(identifier, startChar);
                }
                return new BcsNamedEntityReferenceNode { Identifier = identifier, ExpressioRange = TextRange.FromBounds(startChar, LastChar) };
            });
        }

        private BcsNamedEntityNode ReadComponent(BcsIdentifierNode identifier, int startChar)
        {         
            if (IsPeekType(BcsExpresionTokenType.BracketBegin))
            {
                var component = new BcsStructuralAgentNode
                {
                    Identifier = identifier,
                    Parts = ReadSet(
                        BcsExpresionTokenType.AgentSeparatorOld,
                        ReadAtomicAgent, BcsExpresionTokenType.BracketBegin,
                        BcsExpresionTokenType.BracketEnd,
                        allowEmpty: true),
                    ExpressioRange = TextRange.FromBounds(startChar, LastChar)
                };

                return component;
            }
            return new BcsNamedEntityReferenceNode { Identifier = identifier, ExpressioRange = TextRange.FromBounds(startChar, LastChar) };
        }

        private BcsNamedEntityNode ReadAtomicAgent()
        {
            return ReadVariableOrEntity(() =>
            {
                var startChar = LastChar;
                var identifier = ReadIdentifier();
                return ReadAtomicAgent(identifier, startChar);
            }).CheckExpectedIdentifierError();
        }

        private BcsNamedEntityNode ReadAtomicAgent(BcsIdentifierNode identifier, int startChar)
        {
            if (IsPeekType(BcsExpresionTokenType.SetBegin))
            {
                var agent = new BcsAtomicAgentNode
                {
                    Identifier = identifier,
                    Parts =
                        ReadSet(BcsExpresionTokenType.Comma, ReadState, BcsExpresionTokenType.SetBegin,
                            BcsExpresionTokenType.SetEnd, allowEmpty: true),
                    ExpressioRange = TextRange.FromBounds(startChar, LastChar)
                };

                return agent;
            }
            return new BcsNamedEntityReferenceNode { Identifier = identifier, ExpressioRange = TextRange.FromBounds(startChar, LastChar) };
        }

        private BcsNamedEntityNode ReadState()
        {
            var startChar = LastChar;
            return ReadVariableOrEntity(
                () => new BcsAgentStateNode
                {
                    Identifier = ReadIdentifier(),
                    ExpressioRange = TextRange.FromBounds(startChar, LastChar)
                }).CheckExpectedIdentifierError();
        }

        private void ReadCoeficient(BcsReactantNode reactant)
        {
            if (IsPeekType(BcsExpresionTokenType.ReactionCoeficient))
            {
                var numberStyle = NumberStyles.AllowDecimalPoint;
                double coeficient = 0;
                if (double.TryParse(Peek().Text, numberStyle, CultureInfo.InvariantCulture, out coeficient))
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
            identifier.ExpressioRange = identifier.NameRange;
            return identifier;
        }

        private BcsNamedEntityNode ReadVariableOrEntity<TEntity>(Func<TEntity> entityReadFunc)
            where TEntity : BcsNamedEntityNode
        {
            if (IsPeekType(BcsExpresionTokenType.QuestionMark))
            {
                var startChar = LastChar;
                return new BcsNamedEntityReferenceNode
                {
                    QuestionMark = Read(),
                    Identifier = ReadIdentifier(),
                    ExpressioRange = TextRange.FromBounds(startChar, LastChar)
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
            if (firstElement != null)
            {
                set.Elements.Add(firstElement);
            }

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
            else if (Peek() == null)
            {
                errors.Add(new NodeError($"Unexpected end of the file on index {CurrentIndex}", GetErrorRangeSafe()));
            }
            else
            {

                var errorMessage = $"Unexpected token type: {Peek().Type} containing text: {Peek().Text} on index {CurrentIndex}.";
                errors.Add(new NodeError(errorMessage, GetErrorRangeSafe(), Peek()));
                if (throwException)
                {
                    throw new ParserException(errorMessage);
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

        private TextRange GetErrorRangeSafe()
        {
            return Peek()?.ToTextRange() ?? new TextRange(Math.Max(0, Tokens.LastOrDefault()?.StartPosition ?? 0), (Tokens.LastOrDefault()?.StartPosition ?? 0) + (Tokens.LastOrDefault()?.Length ?? 0));
        }
    }
}
