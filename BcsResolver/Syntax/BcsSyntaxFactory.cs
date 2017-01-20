using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Common;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax
{
    public static class BcsSyntaxFactory
    {
        public static BcsExpressionNode ParseReaction(string text)
        {
            return CreateParser().ParseReaction(CreateTokens(text));
        }

        public static BcsExpressionNode ParseModifier(string text)
        {
            return CreateParser().ParseComplex(CreateTokens(text));
        }

        private static List<BcsExpresionToken> CreateTokens(string text)
        {
            var tokenizer = new BcsExpresionTokenizer();
            tokenizer.Tokenize(new StringReader(text));
            return tokenizer.Tokens;
        }

        private static BcsParser CreateParser()
        {
            return new BcsParser();
        }
    }
}
