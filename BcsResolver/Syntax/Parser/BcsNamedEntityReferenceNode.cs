using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[Ref: {ToDisplayString()}]")]
    public sealed class BcsNamedEntityReferenceNode : BcsNamedEntityNode
    {
        public bool IsVariable => QuestionMark != null;
        public BcsExpresionToken QuestionMark { get; set; }

        public override string ToDisplayString()
        {
            return $"{(QuestionMark!=null?"?":"")}{Identifier.ToDisplayString()}";
        }
    }
}
