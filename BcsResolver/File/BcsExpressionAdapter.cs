﻿using BcsResolver.Parser;
using BcsResolver.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.File
{
    public class BcsExpressionAdapter<TNodeType>
    {
        public TNodeType ExpressionNode { get; set; }
        public string SourceText { get; set; }
        public List<BcsExpresionToken> SourceTokens { get; private set; } = new List<BcsExpresionToken>();
    }
}
