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

        // Emit C++ code
        public override void Emit(Input input)
        {
            input.Output.FunctionDefinitions += Value.Content;
        }
    }
}