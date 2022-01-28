namespace Suteki
{
    class NodeQualifiedName : Node
    {
        public Node Left;
        public Node Right;

        public override string GetString => $"{Left.GetString}.{Right.GetString}";
        public override Token  GetToken  => Token.From(Left.GetToken, GetString);
    }
}