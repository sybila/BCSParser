using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    public class NodeError
    {
        public NodeError(string message, TextRange range, BcsExpresionToken token)
        {
            Message = message;
            Range = range;
            Token = token;
        }

        public NodeError(string message, TextRange range)
            :this(message, range, null)
        { }

        public NodeError(string message)
            :this (message, new TextRange(0, 0) )
        { }

        public string Message { get; private set; }
        public TextRange Range { get; private set; }
        public BcsExpresionToken Token { get; private set; }
    }
}
