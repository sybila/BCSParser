namespace BcsResolver.SemanticModel
{
    public struct SemanticError
    {
        public SemanticError(string message, SemanticErrorSeverity severity)
        {
            Message = message;
            Severity = severity;
        }
        public string Message { get; }
        public SemanticErrorSeverity Severity { get; }
    }
}
