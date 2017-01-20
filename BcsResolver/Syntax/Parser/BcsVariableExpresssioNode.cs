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
        public List<BcsNamedEntityReferenceNode> References { get; set; } =  new List<BcsNamedEntityReferenceNode>();

        public TextRange QuestionMarkOperator { get; set; }
        public TextRange AssignmentOperator { get; set; }
        public List<TextRange> ReferenceSeparators { get; set; } = new List<TextRange>();

        public override string ToDisplayString() => $"{TargetExpression.ToDisplayString()}?{VariableName.ToDisplayString()}={string.Join(",", References.Select(r=> r.ToDisplayString()))}";

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return base.EnumerateChildNodes()
                .Concat(new [] { TargetExpression})
                .Concat(new [] {VariableName})
                .Concat(References);
        }
    }
}
