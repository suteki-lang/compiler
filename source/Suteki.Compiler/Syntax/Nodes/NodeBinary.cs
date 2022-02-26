namespace Suteki.Compiler;

public class NodeBinary : Node
{
    Node         Left;
    Node         Right;
    OperatorKind Op;

    // Constructor
    public NodeBinary(Node left, OperatorKind op, Node right)
    {
        Left  = left;
        Op    = op;
        Right = right;
    }

    public override string   GetString => $"{Left.GetString} {Operator.ToString(Op)} {Right.GetString}";
    public override Token    GetToken  => Token.From(Left.GetToken, GetString);
    public override NodeKind Kind      => NodeKind.Binary;

    // Resolve symbols
    public override void ResolveSymbols(Input input)
    {
        Left .ResolveSymbols(input);
        Right.ResolveSymbols(input);
    }

    // Type checking
    public override Type TypeCheck(Input input)
    {
        // Type check the operands
        Type left  = Left .TypeCheck(input);
        Type right = Right.TypeCheck(input);

        // Compare the types
        if (!left.IsIdentical(right, true))
            input.Logger.Error(GetToken, "Expression types does not match.");

        // Check for operator
        if (Op == OperatorKind.Equality)
            return new TypePrimitive() { Kind = PrimitiveKind.Bool };

        if (left.IsFloat())
            return left;

        return right;
    }

    // Emit C++ code
    public override void Emit(Input input)
    {
        Left.Emit(input);
        input.Output.FunctionDefinitions += $" {Operator.ToString(Op)} ";
        Right.Emit(input);
    }
}