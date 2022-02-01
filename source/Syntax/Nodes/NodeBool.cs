namespace Suteki
{
    class NodeBool : Node
    {
        public Token Value;

        // Constructor
        public NodeBool(Token value)
        {
            Value = value;
        }

        public override string GetString => Value.Content;
        public override Token  GetToken  => Value;

        // Type checking
        public override Type TypeCheck(Input input)
        {
            return new TypePrimitive() 
            {
                Kind = PrimitiveKind.Bool
            };
        }

        // Emit C++ code
        public override void Emit(Input input)
        {
            input.Output.FunctionDefinitions += Value.Content;
        }
    }
}