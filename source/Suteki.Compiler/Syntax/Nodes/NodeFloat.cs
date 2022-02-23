namespace Suteki.Compiler;

public class NodeFloat : Node
{
    public Token Value;

    // Constructor
    public NodeFloat(Token value)
    {
        Value = value;
    }

    public override string   GetString => Value.Content;
    public override Token    GetToken  => Value;
    public override NodeKind Kind      => NodeKind.Float;

    // Type checking
    public override Type TypeCheck(Input input)
    {
        PrimitiveKind kind = PrimitiveKind.Single;

        if (double.Parse(Value.Content) >= float.MaxValue)
            kind = PrimitiveKind.Double;

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