namespace Suteki;

class NodeIdentifierName : Node
{
    public Token Value;

    // Constructor
    public NodeIdentifierName(Token value)
    {
        Value = value;
    }

    public override string   GetString => Value.Content;
    public override Token    GetToken  => Value;
    public override NodeKind Kind      => NodeKind.IdentifierName;

    // Resolve symbols
    public override void ResolveSymbols(Input input)
    {
        if (input.GetSymbol(GetString, GetToken) == null &&
            !input.Locals.ContainsKey(GetString))
            input.Logger.Error(GetToken, "This symbol does not exists.");
    }

    // Type checking
    public override Type TypeCheck(Input input)
    {
        Symbol symbol = input.GetSymbol(GetString, GetToken);

        if (symbol == null)
            symbol = input.Locals[GetString];

        return symbol.Node.TypeCheck(input);
    }

    // Emit C++ code
    public override void Emit(Input input)
    {
        input.Output.FunctionDefinitions += GetString;
    }
}