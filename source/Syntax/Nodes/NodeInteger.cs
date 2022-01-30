namespace Suteki
{
    class NodeInteger : Node
    {
        public Token Value;

        // Constructor
        public NodeInteger(Token value)
        {
            Value = value;
        }

        public override string GetString => Value.Content;
        public override Token  GetToken  => Value;

        // Type checking
        public override ExpressionKind TypeCheck(Input input)
        {
            return ExpressionKind.Integer;
        }

        // Emit C++ code
        public override void Emit(Input input)
        {
            input.Output.FunctionDefinitions += Value.Content;
        }
    }
}