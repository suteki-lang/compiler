namespace Suteki
{
    enum SymbolKind
    {
        None,
        Function,
        Parameter,
    }

    class Symbol
    {
        public SymbolKind   Kind;
        public Module       Module;
        public string       Name;
        public Node         Node;

        // Constructor
        public Symbol(SymbolKind kind, Module module, string name, Node node)
        {
            Kind     = kind;
            Module   = module;
            Name     = name;
            Node     = node;
        }
    }
}