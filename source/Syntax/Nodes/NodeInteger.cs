namespace Suteki;

class NodeInteger : Node
{
    public Token Value;

    // Constructor
    public NodeInteger(Token value)
    {
        Value = value;
    }

    public override string   GetString => Value.Content;
    public override Token    GetToken  => Value;
    public override NodeKind Kind      => NodeKind.Integer;

    // Type checking
    public override Type TypeCheck(Input input)
    {
        long          value = long.Parse(Value.Content);
        PrimitiveKind kind  = ((value < 0) ? PrimitiveKind.SInt : PrimitiveKind.UInt);

        if (value >= int.MaxValue)
            kind = ((value < 0) ? PrimitiveKind.SLong : PrimitiveKind.ULong);

        return new TypePrimitive() 
        {
            Kind = kind
        };
    }

    // Emit C++ code
    public override void Emit(Input input)
    {
        input.Output.FunctionDefinitions += GetString;
    }
}