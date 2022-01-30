namespace Suteki
{
    class NodeQualifiedName : Node
    {
        public Node Left;
        public Node Right;

        // Constructor
        public NodeQualifiedName(Node left, Node right)
        {
            Left  = left;
            Right = right;
        }

        public override string GetString => $"{Left.GetString}.{Right.GetString}";
        public override Token  GetToken  => Token.From(Left.GetToken, GetString);
    }
}