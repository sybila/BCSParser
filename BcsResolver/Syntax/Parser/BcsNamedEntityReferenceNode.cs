using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[Ref: {ToDisplayString()}]")]
    public sealed class BcsNamedEntityReferenceNode : BcsNamedEntityNode
    {
        public override string ToDisplayString()
        {
            return Identifier.ToDisplayString();
        }
    }
}
