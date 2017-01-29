﻿namespace BcsResolver.SemanticModel.Tree
{
    public sealed class ErrorSymbol : BcsNamedSymbol
    {
        public string Error { get; set; }
        public BcsSymbolType ExpectedType { get; set; }

        public ErrorSymbol()
        {
            Type= BcsSymbolType.Error;;
        }
    }
}
