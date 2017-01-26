namespace BcsResolver.SemanticModel
{
    public sealed class BcsLocationSymbol : BcsNamedSymbol
    {
        public BcsLocationSymbol()
        {
            Type = BcsSymbolType.Location;
        }
    }
}
