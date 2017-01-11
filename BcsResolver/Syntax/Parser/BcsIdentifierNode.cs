﻿using System.Collections.Generic;
using System.Diagnostics;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[Id: {ToDisplayString()}]")]
    public class BcsIdentifierNode : BcsExpressionNode
    {
        public string Name { get; set; }
        public TextRange NameRange { get; set; }

        public List<TextRange> WhiteSpacesBefore { get; } = new List<TextRange>();
        public List<TextRange> WhiteSpacesAfter { get; } = new List<TextRange>();

        public override string ToDisplayString() => Name;
    }
}
