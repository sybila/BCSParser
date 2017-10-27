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
        public BcsNamedEntitySet References { get; set; }

        public TextRange DefinitionSeparator { get; set; }
        public TextRange AssignmentOperator { get; set; }

        public override string ToDisplayString() => $"{TargetExpression?.ToDisplayString() ?? ""};{VariableName?.ToDisplayString() ?? ""}={References?.ToDisplayString()?? ""}";

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return base.EnumerateChildNodes()
                .Concat(TargetExpression != null ? new[] { TargetExpression } : Enumerable.Empty<BcsExpressionNode>())
                .Concat(VariableName != null ? new[] { VariableName } : Enumerable.Empty<BcsExpressionNode>())
                .Concat(References != null ? new[] { References } : Enumerable.Empty<BcsExpressionNode>());
        }
    }
}
