namespace Suteki
{
    class NodeString : Node
    {
        public Token Value;

        // Constructor
        public NodeString(Token value)
        {
            Value = value;
        }

        public override string GetString => Value.Content;
        public override Token  GetToken  => Value;

        // Type checking
        public override ExpressionKind TypeCheck(Input input)
        {
            return ExpressionKind.String;
        }

        // Emit C++ code
        public override void Emit(Input input)
        {
            input.Output.FunctionDefinitions += Value.Content;
        }
    }
}