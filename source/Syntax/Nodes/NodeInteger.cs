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
        public override Type TypeCheck(Input input)
        {
            PrimitiveKind kind = PrimitiveKind.Int;

            if (long.Parse(Value.Content) >= int.MaxValue)
                kind = PrimitiveKind.Long;

            return new TypePrimitive() 
            {
                Kind = kind
            };
        }

        // Emit C++ code
        public override void Emit(Input input)
        {
            input.Output.FunctionDefinitions += Value.Content;
        }
    }
}