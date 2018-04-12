using System.Collections.Generic;
using BcsResolver.SemanticModel.BoundTree;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.SemanticModel
{
    public class ReactionModel
    {
        public string SourceText { get; }
        public BcsExpressionNode Tree { get; }
        public IBcsBoundSymbol SemanticTree { get; }
        public IList<ModelError> Errors { get; }

        public ReactionModel(string sourceText,
            BcsExpressionNode tree, 
            IBcsBoundSymbol semanticTree,
            IList<ModelError> errors
            )
        {
            SourceText = sourceText;
            Tree = tree;
            SemanticTree = semanticTree;
            Errors = errors;
        }
    }

    public class ModelError
    {
        public ModelErrorType Type { get; }
        public string Message { get; }
        public TextRange TextRange { get; }

        public ModelError(ModelErrorType type, string message, TextRange textRange)
        {
            Type = type;
            Message = message;
            TextRange = textRange;
        }
    }

    public enum ModelErrorType
    {
        Token,
        Syntax,
        Semantic
    }
}