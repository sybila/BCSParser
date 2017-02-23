using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Extensions
{
    public static class TextExtensions
    {   
        public static bool ContainsPosition(this TextRange range, int position)
        {
            return range.Start <= position && position < range.End;
        }

        public static bool ContainsRange(this TextRange token, TextRange range)
        {
            return token.ContainsPosition(range.Start) && range.End <= token.End;
        }

        public static TextRange RangeFromBounds(int start, int end)
        {
            return new TextRange(start, end - start);
        }

        public static TextRange Offset(this TextRange range, int offset)
        {
            return new TextRange(offset + range.Start, range.Length);
        }
        public static string RemoveAllWhitespaces (this string str)
        {
            return Regex.Replace(str, @"\s+", "");
        }

        public static bool CaselessEquals(this string str, string reference)
        {
            return str.Equals(reference, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsEmptyOrWhitespace(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }

        public static TNode CheckExpectedIdentifierError<TNode>(this TNode hostNode)
            where TNode : BcsNamedEntityNode
        {
            if (hostNode.Identifier == null)
            {
                hostNode.Errors.Add(new NodeError("Identifier expected."));
            }
            return hostNode;
        }
    }
}
