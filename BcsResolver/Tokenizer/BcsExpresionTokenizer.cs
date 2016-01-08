using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.Tokenizer
{
    public class BcsExpresionTokenizer : TokenizerBase<BcsExpresionToken, BcsExpresionTokenType>
    {
        protected override BcsExpresionTokenType TextTokenType => BcsExpresionTokenType.AgentIdentifier;

        protected override BcsExpresionTokenType WhiteSpaceTokenType => BcsExpresionTokenType.Whitespace;

        private char[] identifierStopChars = new char[] { ':', '{', '(', '<', '=', '+', '.'};

        protected override void TokenizeCore()
        {
            while (Peek() != NullChar)
            {
                SkipWhitespace();
                ReadReaction();
            }
        }

        private void ReadReaction()
        {
            ReadReactionFormula();

            if (Peek() != NullChar)
            {
                ReadReactionDirection();
            }

            SkipWhitespace();

            if (Peek() != NullChar)
            {
                ReadReactionFormula();
            }
        }

        private void ReadReactionFormula()
        {
            //Read until reaction direction mark or ond of line or file is found
            while (Peek() != '<' && Peek() != '=' && Peek() != NullChar)
            {
                ReadReactant();
                //dont eat newline
                SkipWhitespace(false);

                if (Peek() == '+')
                {
                    Read();
                    CreateToken(BcsExpresionTokenType.AgentInteractionPositive);
                    SkipWhitespace(false);
                }
                else if (Peek() == '\n' || Peek() == '\r')
                {
                    SkipWhitespace();
                    break;
                }
            }
        }

        private void ReadReactant()
        {
            ReadCoeficient();
            SkipWhitespace();
            CreateToken(BcsExpresionTokenType.ComplexStart);
            ReadComposition();
            SkipWhitespace();
            while (Peek() == '.')
            {
                Read();
                CreateToken(BcsExpresionTokenType.CompositionSeparator);
                SkipWhitespace();
                ReadComposition();
                SkipWhitespace();
            }
            CreateToken(BcsExpresionTokenType.ComplexEnd);
        }

        private void ReadCoeficient()
        {
            bool decimalDotFound = false;

            while (Peek() != NullChar)
            {
                char possibleNumber = Peek();
                int outputInt = 0;

                if (int.TryParse(possibleNumber.ToString(), out outputInt) || (Peek() == '.' && !decimalDotFound))
                {
                    if (Peek() == '.') { decimalDotFound = true; }
                    Read();
                }
                else { break; }
            }

            if (DistanceSinceLastToken > 0)
            {
                CreateToken(BcsExpresionTokenType.ReactionCoeficient);
            }
        }

        private void ReadComposition()
        {
            ReadComponent();

            while (Peek() == ':')
            {
                ReadInheritanceMark();

                ReadComponent();
            }
        }

        private void ReadComponent()
        {
            CreateToken(BcsExpresionTokenType.ComponentStart);
            ReadTextUntilNewLine(BcsExpresionTokenType.AgentIdentifier, identifierStopChars);

            if (Peek() == '{')
            {
                ReadAtomicAgentState();
            }
            else if (Peek() == '(')
            {
                ReadParcialComposition();
            }
            CreateToken(BcsExpresionTokenType.ComponentEnd);
        }

        private void ReadParcialComposition()
        {
            Read();
            CreateToken(BcsExpresionTokenType.CompositionBegin);

            ReadTextUntilNewLine(BcsExpresionTokenType.AgentIdentifier, new char[] { '{' });
            ReadAtomicAgentState();

            while (Peek() == '|')
            {
                Read();
                CreateToken(BcsExpresionTokenType.AgentSeparator);
                ReadTextUntilNewLine(BcsExpresionTokenType.AgentIdentifier, new char[] { '{' });
                ReadAtomicAgentState();
            }

            Assert(Peek() == ')');
            Read();
            CreateToken(BcsExpresionTokenType.CompositionEnd);
        }

        private void ReadAtomicAgentState()
        {
            Read();
            CreateToken(BcsExpresionTokenType.AgentStateBegin);
            SkipWhitespace();
            ReadTextUntilNewLine(BcsExpresionTokenType.AgentStateIdentifier, new char[] { '}' });
            Assert(Peek() == '}');
            Read();
            CreateToken(BcsExpresionTokenType.AgentStateEnd);
            SkipWhitespace();
        }

        private void ReadInheritanceMark()
        {
            Assert(Peek() == ':');
            Read();
            Assert(Peek() == ':');
            Read();
            CreateToken(BcsExpresionTokenType.InheritanceMark);
            SkipWhitespace();
        }

        private void ReadReactionDirection()
        {
            Assert(Peek() == '=' || Peek() == '<');
            if (Peek() == '<')
            {
                Read();
                Assert(Peek() == '=');
                Read();
                if (Peek() == '>')
                {
                    Read();
                    CreateToken(BcsExpresionTokenType.ReactionDirectionBoth);
                }
                else
                {
                    CreateToken(BcsExpresionTokenType.ReactionDirectionLeft);
                }
            }
            else if (Peek() == '=')
            {
                Read();
                Assert(Peek() == '>');
                Read();
                CreateToken(BcsExpresionTokenType.ReactionDirectionRight);
            }
        }
    }
}
