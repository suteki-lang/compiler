namespace Suteki;

class NodePointer : Node
{
    public Node PointsTo;
    
    public override string   GetString => $"{PointsTo.GetString}*";
    public override Token    GetToken  => PointsTo.GetToken;
    public override NodeKind Kind      => NodeKind.Pointer;

    // Type checking
    public override Type TypeCheck(Input input)
    {
        return new TypePointer() 
        {
            Base = PointsTo.TypeCheck(input)
        };
    }
}