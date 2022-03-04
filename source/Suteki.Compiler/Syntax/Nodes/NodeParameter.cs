namespace Suteki.Compiler;

public class NodeParameter : Node
{
    public Node  Type;
    public Token Name;

    public override string   GetString => $"{Type.GetString}{Name.Content}";
    public override Token    GetToken  => Name;
    public override NodeKind Kind      => NodeKind.Parameter;

    // Resolve symbols
    public override void ResolveSymbols(Input input)
    {
        // Check for local symbol
        if (input.GetSymbol(Name.Content) != null)
        {
            input.Logger.Error(Name, "This symbol already exists.");
            return;
        }

        // Add local symbol
        input.Locals.Add(Name.Content, new Symbol(SymbolKind.Parameter, PropertyKind.None, input.Module, Name.Content));
    }

    // Resolve types
    public override Type ResolveTypes(Input input)
    {
        Symbol symbol      = input.Locals[Name.Content];
               symbol.Type = Type.TypeCheck(input);

        return symbol.Type;
    }

    // Type checking
    public override Type TypeCheck(Input input)
    {
        return input.Locals[Name.Content].Type;
    }
}