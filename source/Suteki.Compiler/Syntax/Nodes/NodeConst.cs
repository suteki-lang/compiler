namespace Suteki.Compiler;

public class NodeConst : Node
{
    public Token Token;
    public Node  Type;
    
    public override string   GetString => $"const {Type.GetString}";
    public override Token    GetToken  => Token;
    public override NodeKind Kind      => NodeKind.Const;

    // Type checking
    public override Type TypeCheck(Input input)
    {
        return new TypeConst() 
        {
            Type = Type.TypeCheck(input)
        };
    }
}