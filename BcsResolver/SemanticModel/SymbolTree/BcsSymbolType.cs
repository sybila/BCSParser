using System.ComponentModel;

namespace BcsResolver.SemanticModel.Tree
{
    public enum BcsSymbolType
    {
        [Description("Complex")]
        Complex = 0,
        [Description("Structural agent")]
        StructuralAgent = 1,
        [Description("Atomic agent")]
        Agent = 2,
        [Description("Atomic agent state")]
        State = 3,
        [Description("Location")]
        Location = 4,
        [Description("Rule")]
        Rule = 5,
        [Description("Unknown entity")]
        Unknown = 53,
        [Description("Entity error")]
        Error = 9,
    }
}
