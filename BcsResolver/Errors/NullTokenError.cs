using BcsResolver.Tokenizer;

namespace BcsResolver.Errors
{
    public class NullTokenError<TToken, TTokenType> : TokenError<TToken, TTokenType> where TToken : TokenBase<TTokenType>, new()
    {
        public NullTokenError(TokenizerBase<TToken, TTokenType> tokenizer) : base("", tokenizer)
        {
        }

        protected override TextRange GetRange()
        {
            return new TextRange();
        }
    }
}