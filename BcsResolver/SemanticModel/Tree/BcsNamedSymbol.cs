namespace BcsResolver.SemanticModel.Tree
{
    public abstract class BcsNamedSymbol : BcsSymbol
    {
        public string Name { get; set; }

        public override string ToDisplayString() => $"{base.ToDisplayString()}: {Name}";
    }
}
