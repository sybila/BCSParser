using BcsResolver.Tokenizer;

namespace BcsResolver.Errors
{
    public abstract class TokenError
    {

        public string ErrorMessage { get; private set; }

        public abstract TextRange Range { get; }

        protected TokenError(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

    }

    public abstract class TokenError<TToken, TTokenType> : TokenError where TToken : TokenBase<TTokenType>, new()
    {
        public TokenizerBase<TToken, TTokenType> Tokenizer { get; private set; }

        private TextRange? range = null;
        public override TextRange Range
        {
            get { return range ?? (range = GetRange()).Value; }
        }


        public TokenError(string errorMessage, TokenizerBase<TToken, TTokenType> tokenizer) : base(errorMessage)
        {
            Tokenizer = tokenizer;
        }

        protected abstract TextRange GetRange();

    }
}