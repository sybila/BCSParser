using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.Extensions
{
    public static class TextExtensions
    {
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
