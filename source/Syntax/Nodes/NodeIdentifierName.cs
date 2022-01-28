namespace Suteki
{
    class NodeIdentifierName : Node
    {
        public Token Value;

        // Constructor
        public NodeIdentifierName(Token value)
        {
            Value = value;
        }

        public override string GetString => Value.Content;
        public override Token  GetToken  => Value;
    }
}