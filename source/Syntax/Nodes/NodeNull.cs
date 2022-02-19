namespace Suteki
{
    class NodeNull : Node
    {
        public Token Value;

        // Constructor
        public NodeNull(Token value)
        {
            Value = value;
        }

        public override string   GetString => Value.Content;
        public override Token    GetToken  => Value;
        public override NodeKind Kind      => NodeKind.Null;

        // Type checking
        public override Type TypeCheck(Input input)
        {
            return new TypePrimitive()
            {
                Kind = PrimitiveKind.Null
            };
        }

        // Emit C++ code
        public override void Emit(Input input)
        {
            input.Output.FunctionDefinitions += "nullptr";
        }
    }
}