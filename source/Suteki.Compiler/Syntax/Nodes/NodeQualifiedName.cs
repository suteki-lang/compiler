namespace Suteki.Compiler;

public class NodeQualifiedName : Node
{
    public Node Left;
    public Node Right;

    // Constructor
    public NodeQualifiedName(Node left, Node right)
    {
        Left  = left;
        Right = right;
    }

    public override string GetString => $"{Left.GetString}.{Right.GetString}";
    public override Token  GetToken  => Token.From(Left.GetToken, GetString);
    public override NodeKind Kind    => NodeKind.QualifiedName;

    // Resolve symbols
    public override void ResolveSymbols(Input input)
    {
        if (input.GetSymbol(GetString, GetToken) == null)
            input.Logger.Error(GetToken, "This symbol does not exists.");
    }

    // Type checking
    public override Type TypeCheck(Input input)
    {
        Symbol symbol = input.GetSymbol(GetString, GetToken);

        if (symbol == null)
            symbol = input.Locals[GetString];

        return symbol.Type;
    }

    // Emit C++ code
    public override void Emit(Input input)
    {
        input.Output.FunctionDefinitions += GetString;
    }
}