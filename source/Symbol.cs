namespace Suteki
{
    enum SymbolKind
    {
        None,
        Function,
    }

    class Symbol
    {
        public SymbolKind   Kind;
        public Module       Module;
        public Node         Node;

        // Constructor
        public Symbol(SymbolKind kind, Module module, Node node)
        {
            Kind     = kind;
            Module   = module;
            Node     = node;
        }
    }
}