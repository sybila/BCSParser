namespace BcsResolver.Syntax.Tokenizer
{
    public struct TextRange
    {
        public int StartPosition { get; private set; }

        public int Length { get; private set; }

        public int End => StartPosition + Length;

        public TextRange(int startPosition, int length)
        {
            StartPosition = startPosition;
            Length = length;
        }
    }
}