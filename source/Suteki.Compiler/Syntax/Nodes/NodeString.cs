namespace Suteki.Compiler;

public class NodeString : Node
{
    public Token Value;

    // Constructor
    public NodeString(Token value)
    {
        Value = value;
    }

    public override string   GetString => Value.Content;
    public override Token    GetToken  => Value;
    public override NodeKind Kind      => NodeKind.String;

    // Type checking
    public override Type TypeCheck(Input input)
    {
        return new TypePrimitive()
        {
            Kind = PrimitiveKind.String
        };
    }

    // Emit C++ code
    public override void Emit(Input input)
    {
        input.Output.FunctionDefinitions += GetString;
    }
}