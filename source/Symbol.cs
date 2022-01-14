namespace Suteki
{
    enum SymbolKind
    {
        None,
        Function,
    }

    class Symbol
    {
        public SymbolKind Kind;

        // Symbol Constructor
        public Symbol(SymbolKind kind)
        {
            Kind = kind;
        }
    }
}