using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[VE: {ToDisplayString()}]")]
    public class BcsVariableExpresssioNode : BcsExpressionNode
    {
        public BcsExpressionNode TargetExpression { get; set; }
        public BcsIdentifierNode VariableName { get; set; }
        public BcsSet<BcsNamedEntityNode> References { get; set; }

        public TextRange DefinitionSeparator { get; set; }
        public TextRange AssignmentOperator { get; set; }

        public override string ToDisplayString() => $"{TargetExpression.ToDisplayString()};{VariableName.ToDisplayString()}={References.ToDisplayString()}";

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return base.EnumerateChildNodes()
                .Concat(new[] { TargetExpression })
                .Concat(new[] { VariableName })
                .Concat(new[] { References });
        }
    }
}
