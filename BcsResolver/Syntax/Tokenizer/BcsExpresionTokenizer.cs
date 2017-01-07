using System.Collections.Generic;
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
                        CreateToken(BcsExpresionTokenType.ComponentBegin);
                        break;

                    case ')':
                        FinishIncompleteIdentifier();
                        Read();
                        CreateToken(BcsExpresionTokenType.ComponentEnd);
                        break;
                    case '{':
                        FinishIncompleteIdentifier();
                        Read();
                        CreateToken(BcsExpresionTokenType.AgentBegin);
                        break;

                    case '}':
                        FinishIncompleteIdentifier();
                        Read();
                        CreateToken(BcsExpresionTokenType.AgentEnd);
                        break;
                    case '+':
                        FinishIncompleteIdentifier();
                        Read();
                        EnsureUnsupportedOperator(BcsExpresionTokenType.Interaction);
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
                            EnsureUnsupportedOperator(BcsExpresionTokenType.Invalid);
                        }
                        break;
                    case '|':
                        FinishIncompleteIdentifier();
                        Read();
                        EnsureUnsupportedOperator(BcsExpresionTokenType.AgentSeparator);
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
                        if (char.IsWhiteSpace(ch))
                        {
                            // white space
                            FinishIncompleteIdentifier();
                            SkipWhitespace();
                        }
                        else if (char.IsLetter(ch) && IsCurrentTokenNumeric(canContainDot: true))
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
            if (DistanceSinceLastToken > 0)
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
