namespace Suteki.Compiler;

public class NodeIdentifierName : Node
{
    public Token Value;

    // Constructor
    public NodeIdentifierName(Token value)
    {
        Value = value;
    }

    public override bool     IsIdentifier  => true;
    public override string   GetString     => Value.Content;
    public override string   GetIdentifier => Value.Content;
    public override Token    GetToken      => Value;
    public override NodeKind Kind          => NodeKind.IdentifierName;

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
        return symbol.Type;
    }

    // Emit C++ code
    public override void Emit(Input input)
    {
        Symbol symbol = input.GetSymbol(GetString);
        string name   = GetString;
        
        if (symbol.Kind != SymbolKind.Parameter)
            name = Config.MangleName(GetString, symbol.Property, symbol);

        input.Output.FunctionDefinitions += name;
    }
}