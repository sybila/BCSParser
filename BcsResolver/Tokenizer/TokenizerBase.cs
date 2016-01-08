using BcsResolver.Common;
using BcsResolver.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BcsResolver.Tokenizer
{
    public abstract class TokenizerBase<TToken, TTokenType> where TToken : TokenBase<TTokenType>, new()
    {

        public const char NullChar = '\0';
        private IReader reader;


        /// <summary>
        /// Gets the type of the text token.
        /// </summary>
        protected abstract TTokenType TextTokenType { get; }

        /// <summary>
        /// Gets the type of the white space token.
        /// </summary>
        protected abstract TTokenType WhiteSpaceTokenType { get; }


        /// <summary>
        /// Gets or sets the current line number.
        /// </summary>
        protected int CurrentLine { get; private set; }

        /// <summary>
        /// Gets or sets the position on current line.
        /// </summary>
        protected int PositionOnLine { get; private set; }

        /// <summary>
        /// Gets the last token position.
        /// </summary>
        protected int LastTokenPosition { get; private set; }

        /// <summary>
        /// Gets the last token.
        /// </summary>
        protected TToken LastToken { get; private set; }

        /// <summary>
        /// Gets the distance since last token.
        /// </summary>
        protected int DistanceSinceLastToken
        {
            get { return reader.Position - LastTokenPosition; }
        }

        /// <summary>
        /// Gets or sets the current token chars.
        /// </summary>
        protected StringBuilder CurrentTokenChars { get; private set; }

        /// <summary>
        /// Occurs when a token is found.
        /// </summary>
        public event Action<TToken> TokenFound;

        /// <summary>
        /// Gets the list of tokens.
        /// </summary>
        public List<TToken> Tokens { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="TokenizerBase{TToken, TTokenType}"/> class.
        /// </summary>
        public TokenizerBase()
        {
            CurrentTokenChars = new StringBuilder();
            Tokens = new List<TToken>();
        }


        /// <summary>
        /// Tokenizes the input.
        /// </summary>
        public void Tokenize(IReader reader)
        {
            this.reader = reader;

            try
            {
                CurrentLine = 1;
                PositionOnLine = 0;
                LastToken = null;
                LastTokenPosition = 0;
                Tokens.Clear();
                CurrentTokenChars.Clear();

                TokenizeCore();
            }
            finally
            {
                reader.Dispose();
            }
        }

        /// <summary>
        /// Tokenizes the core.
        /// </summary>
        protected abstract void TokenizeCore();


        /// <summary>
        /// Skips the whitespace.
        /// </summary>
        protected void SkipWhitespace(bool allowEndLine = true)
        {
            while (char.IsWhiteSpace(Peek()) && (allowEndLine || (Peek() != '\r' && Peek() != '\n')))
            {
                if (Read() == NullChar)
                {
                    break;
                }
            }
            if (DistanceSinceLastToken > 0)
            {
                CreateToken(WhiteSpaceTokenType);
            }
        }

        /// <summary>
        /// Skips the until new line or when it hits the specified stop chars. 
        /// When the new line is hit, the method automatically consumes it and creates WhiteSpace token.
        /// When the stopchar is hit, it is not consumed.
        /// </summary>
        protected void ReadTextUntilNewLine(TTokenType tokenType, params char[] stopChars)
        {
            int lastNonwhitespaceDistance = 0;

            while (Peek() != '\r' && Peek() != '\n' && !stopChars.Contains(Peek()))
            {
                lastNonwhitespaceDistance = char.IsWhiteSpace(Peek()) ? lastNonwhitespaceDistance + 1 : 0;
                if (Read() == NullChar)
                {
                    break;
                }
            }
            if (DistanceSinceLastToken > 0)
            {
                CreateToken(tokenType, lastNonwhitespaceDistance);
            }

            if (Peek() == '\r')
            {
                // \r can be followed by \n which is still one new line
                Read();
            }
            if (Peek() == '\n')
            {
                Read();
            }

            if (DistanceSinceLastToken > 0)
            {
                CreateToken(WhiteSpaceTokenType);
            }
        }


        protected bool ReadTextUntil(TTokenType tokenType, string stopString, bool stopOnNewLine)
        {
            var startLine = CurrentLine;
            var stopStringIndex = 0;
            while (stopStringIndex != stopString.Length)
            {
                var ch = Read();
                if (ch == NullChar)
                {
                    return false;
                }
                else if (ch == stopString[stopStringIndex])
                {
                    stopStringIndex++;
                }
                else
                {
                    stopStringIndex = 0;
                }

                if (stopOnNewLine && startLine != CurrentLine)
                {
                    return false;
                }
            }

            CreateToken(tokenType, stopString.Length);
            return true;
        }

        protected string ReadOneOf(params string[] strings)
        {
            int index = 0;
            while (strings.Length > 0 && !strings.Any(s => s.Length <= index))
            {
                var ch = Peek();
                strings = strings.Where(s => s[index] == ch).ToArray();
                if (strings.Length == 0) return null;
                index++;
                Read();
            }
            return strings.FirstOrDefault(s => s.Length == index);
        }

        /// <summary>
        /// Creates the token.
        /// </summary>
        protected TToken CreateToken(TTokenType type, int charsFromEndToSkip = 0, Func<TToken, TokenError> errorProvider = null)
        {
            LastToken = new TToken()
            {
                LineNumber = CurrentLine,
                ColumnNumber = Math.Max(0, PositionOnLine - DistanceSinceLastToken - 1),
                StartPosition = LastTokenPosition,
                Length = DistanceSinceLastToken - charsFromEndToSkip,
                Type = type,
                Text = CurrentTokenChars.ToString().Substring(0, DistanceSinceLastToken - charsFromEndToSkip)
            };
            Tokens.Add(LastToken);
            if (errorProvider != null)
            {
                LastToken.Error = errorProvider(LastToken);
            }

            CurrentTokenChars.Remove(0, LastToken.Length);
            LastTokenPosition = reader.Position - charsFromEndToSkip;

            OnTokenFound(LastToken);

            return LastToken;
        }

        protected TokenError CreateTokenError()
        {
            return new NullTokenError<TToken, TTokenType>(this);
        }

        protected TokenError CreateTokenError(TToken lastToken, TTokenType firstTokenType, string errorMessage)
        {
            return new BeginWithLastTokenOfTypeTokenError<TToken, TTokenType>(errorMessage, this, lastToken, firstTokenType);
        }

        protected TokenError CreateTokenError(TToken token, string errorMessage)
        {
            return new SimpleTokenError<TToken, TTokenType>(errorMessage, this, token);
        }
        /// <summary>
        /// Called when a token is found.
        /// </summary>
        protected virtual void OnTokenFound(TToken token)
        {
            var handler = TokenFound;
            if (handler != null)
            {
                handler(token);
            }
        }


        /// <summary>
        /// Peeks the current char.
        /// </summary>
        protected char Peek()
        {
            return reader.Peek();
        }

        /// <summary>
        /// Returns the current char and advances to the next one.
        /// </summary>
        protected char Read()
        {
            var ch = reader.Read();
            if (ch != NullChar)
            {
                CurrentTokenChars.Append(ch);

                if (ch == '\r' && Peek() != '\n')
                {
                    CurrentLine++;
                    PositionOnLine = 0;
                }
                else if (ch == '\n')
                {
                    CurrentLine++;
                    PositionOnLine = 0;
                }
            }

            PositionOnLine++;
            return ch;
        }

        protected void Assert(bool expression)
        {
            if (!expression)
            {
                throw new Exception($"Tokenizer Internal Error: Assertion failed! At line {CurrentLine}, position {PositionOnLine}, symbol: '{Peek()}'.");
            }
        }
    }
}