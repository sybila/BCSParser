using BcsResolver.Errors;

namespace BcsResolver.Syntax.Tokenizer
{
    public abstract class TokenBase
    {

        public int StartPosition { get; set; }

        public int Length { get; set; }

        public string Text { get; set; }

        public int LineNumber { get; set; }

        public int ColumnNumber { get; set; }

        public TokenError Error { get; set; }

        public bool HasError => Error != null;

        public override string ToString()
        {
            return $"Token ({StartPosition}:{Length}): {Text}";
        }
    }

    public abstract class TokenBase<TTokenType> : TokenBase
    {

        public TTokenType Type { get; set; }

        public override string ToString()
        {
            return $"{Type} ({StartPosition}:{Length}): {Text}";
        }
    }
}