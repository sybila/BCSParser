namespace BcsResolver.SemanticModel.Tree
{
    public sealed class BcsErrorSymbol : BcsNamedSymbol
    {
        public string Error { get; set; }
        public BcsSymbolType ExpectedType { get; set; }

        public BcsErrorSymbol()
        {
            Type= BcsSymbolType.Error;;
        }
    }
}
