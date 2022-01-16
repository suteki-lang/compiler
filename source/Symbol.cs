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
        public Node       Node;

        // Symbol Constructor
        public Symbol(SymbolKind kind, Node node)
        {
            Kind = kind;
            Node = node;
        }

        // Get expression kind
        public ExpressionKind GetExpressionKind(Input input)
        {
            switch (Kind)
            {
                case SymbolKind.Function:
                    return ((NodeFunction)Node).Type.TypeCheck(input);

                default:
                    return ExpressionKind.Void;
            }
        }
    }
}