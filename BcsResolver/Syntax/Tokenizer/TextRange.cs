using System;

namespace BcsResolver.Syntax.Tokenizer
{
    public struct TextRange
    {
        public int Start { get; private set; }

        public int Length { get; private set; }

        public int End => Start + Length;

        public TextRange(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public static TextRange FromBounds(int start, int end)
        {
            if (end - start < 0) { throw new ArgumentException("End must be after start"); }
            return new TextRange(start, end - start);
        }

    }
}