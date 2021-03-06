﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Extensions
{
    public static class BcsExpressionTokenExtensions
    {
        public static TextRange ToTextRange(this BcsExpresionToken token)
        {
            return new TextRange(token.StartPosition, token.Length);
        }
    }
}
