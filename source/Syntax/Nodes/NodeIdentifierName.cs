namespace Suteki
{
    class NodeIdentifierName : Node
    {
        public Token Value;

        public override string GetString => Value.Content;
        public override Token  GetToken  => Value;
    }
}