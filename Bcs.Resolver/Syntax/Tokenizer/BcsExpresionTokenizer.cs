﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BcsResolver.Syntax.Tokenizer
{
    public class BcsExpresionTokenizer : TokenizerBase<BcsExpresionToken, BcsExpresionTokenType>
    {
        private readonly ISet<char> operatorCharacters = new HashSet<char> { '+', '-', '*', '/', '^', '\\', '%', '<', '>', '=', '&', '|', '~', '!' };

        protected override BcsExpresionTokenType TextTokenType => BcsExpresionTokenType.Identifier;

        protected override BcsExpresionTokenType WhiteSpaceTokenType => BcsExpresionTokenType.Whitespace;

        protected override void TokenizeCore()
        {
            while (Peek() != NullChar)
            {
                SkipWhitespace();
                ReadReaction();
            }
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private void ReadReaction()
        {
            while (Peek() != NullChar)
            {
                var ch = Peek();

                switch (ch)
                {
                    case '.':
                        if (IsCurrentTokenNumeric(canContainDot: false))
                        {
                            Read();
                            if (!char.IsDigit(Peek()))
                            {
                                CreateToken(BcsExpresionTokenType.Identifier, 1);
                                CreateToken(BcsExpresionTokenType.Dot);
                            }
                        }
                        else
                        {
                            FinishIncompleteIdentifier();
                            Read();
                            CreateToken(BcsExpresionTokenType.Dot);
                        }
                        break;

                    case ',':
                        FinishIncompleteIdentifier();
                        Read();
                        CreateToken(BcsExpresionTokenType.Comma);
                        break;

                    case '(':
                        FinishIncompleteIdentifier();
                        Read();
                        CreateToken(BcsExpresionTokenType.BracketBegin);
                        break;

                    case ')':
                        FinishIncompleteIdentifier();
                        Read();
                        CreateToken(BcsExpresionTokenType.BracketEnd);
                        break;
                    case '{':
                        FinishIncompleteIdentifier();
                        Read();
                        CreateToken(BcsExpresionTokenType.SetBegin);
                        break;

                    case '}':
                        FinishIncompleteIdentifier();
                        Read();
                        CreateToken(BcsExpresionTokenType.SetEnd);
                        break;
                    case ';':
                        FinishIncompleteIdentifier();
                        Read();
                        EnsureUnsupportedOperator(BcsExpresionTokenType.Semicolon);
                        break;
                    case ':':
                        FinishIncompleteIdentifier();
                        Read();
                        if (Peek() == ':')
                        {
                            Read();
                            EnsureUnsupportedOperator(BcsExpresionTokenType.FourDot);
                        }
                        else
                        {
                            Read();
                            EnsureUnsupportedOperator(BcsExpresionTokenType.Invalid);
                        }
                        break;
                    case '=':
                        FinishIncompleteIdentifier();
                        Read();
                        if (Peek() == '>')
                        {
                            Read();
                            EnsureUnsupportedOperator(BcsExpresionTokenType.ReactionDirectionRight);
                        }
                        else
                        {
                            EnsureUnsupportedOperator(BcsExpresionTokenType.Assignment);
                        }
                        break;
                    case '?':
                        FinishIncompleteIdentifier();
                        Read();
                        EnsureUnsupportedOperator(BcsExpresionTokenType.QuestionMark);
                        break;
                    case '|':
                        FinishIncompleteIdentifier();
                        Read();
                        EnsureUnsupportedOperator(BcsExpresionTokenType.AgentSeparatorOld);
                        break;
                    case '<':
                        FinishIncompleteIdentifier();
                        Read();
                        if (Peek() == '=')
                        {
                            Read();
                            if (Peek() == '>')
                            {
                                Read();
                                EnsureUnsupportedOperator(BcsExpresionTokenType.ReactionDirectionBoth);
                            }
                            else
                            {
                                EnsureUnsupportedOperator(BcsExpresionTokenType.ReactionDirectionLeft);
                            }
                        }
                        else
                        {
                            EnsureUnsupportedOperator(BcsExpresionTokenType.Invalid);
                        }
                        break;
                    default:
                        //We also read + sign + is normal identifier like any other
                        if (char.IsWhiteSpace(ch))
                        {
                            // white space
                            FinishIncompleteIdentifier();
                            SkipWhitespace();
                        }
                        else if ((char.IsLetter(ch) || ch=='+') && IsCurrentTokenNumeric(canContainDot: true))
                        {
                            //finish number
                            FinishIncompleteIdentifier();
                            SkipWhitespace();
                        }
                        else
                        {
                            // text content
                            Read();
                        }
                        break;
                }
            }

            // treat remaining content as text
            FinishIncompleteIdentifier();
        }

        private void FinishIncompleteIdentifier()
        {
            if (IsTokenInProgress())
            {
                if (IsCurrentTokenNumeric(canContainDot: true))
                {
                    CreateToken(BcsExpresionTokenType.ReactionCoeficient);
                }
                else
                {
                    CreateToken(BcsExpresionTokenType.Identifier);
                }
            }
        }

        private bool IsTokenInProgress()
        {
            return DistanceSinceLastToken > 0;
        }

        private bool IsIdentifierInProgress()
            => IsTokenInProgress()
            && !IsCurrentTokenNumeric(canContainDot: true);

        internal void EnsureUnsupportedOperator(BcsExpresionTokenType preferedOperatorToken)
        {
            if (IsOperator(Peek()))
            {
                while (IsOperator(Peek()))
                {
                    Read();
                }
                CreateToken(BcsExpresionTokenType.Invalid);
            }
            else
            {
                CreateToken(preferedOperatorToken);
            }
        }

        private bool IsCurrentTokenNumeric(bool canContainDot)
        {
            return CurrentTokenChars.Length > 0 &&
                Enumerable.Range(0, CurrentTokenChars.Length).All(i => char.IsDigit(CurrentTokenChars[i]) || canContainDot && CurrentTokenChars[i] == '.');
        }

        internal bool IsOperator(char c) => operatorCharacters.Contains(c);

    }
}
