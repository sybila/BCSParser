namespace BcsResolver.SemanticModel.Tree
{
    public abstract class BcsNamedSymbol : BcsSymbol
    {
        public string Name { get; set; }
        public string FullName { get; set; }

        public override string ToDisplayString() => $"{base.ToDisplayString()}: {Name}";
    }
}
