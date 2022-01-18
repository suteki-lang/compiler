namespace Suteki
{
    enum SymbolKind
    {
        None,
        Function,
    }

    class Symbol
    {
        public PropertyKind Property;
        public SymbolKind   Kind;
        public Node         Node;

        // Symbol Constructor
        public Symbol(PropertyKind property, SymbolKind kind, Node node)
        {
            Property = property;
            Kind     = kind;
            Node     = node;
        }

        // Get function node
        public NodeFunction GetNodeFunction()
        {
            return Node as NodeFunction;
        }

        // Get expression kind
        public ExpressionKind GetExpressionKind(Input input)
        {
            switch (Kind)
            {
                case SymbolKind.Function:
                    return GetNodeFunction().Type.TypeCheck(input);

                default:
                    return ExpressionKind.Void;
            }
        }
    }
}