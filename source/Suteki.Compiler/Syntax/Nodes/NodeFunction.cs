namespace Suteki.Compiler;

using System.Collections.Generic;

public class NodeFunction : Node
{
    public PropertyKind Property;
    public Node         Type;
    public Token        Name;
    public List<Node>   Parameters = new List<Node>();
    public Node         Body;

    public override Token    GetToken => Name;
    public override NodeKind Kind      => NodeKind.Function;

    // Register symbols
    public override void RegisterSymbols(Input input)
    {
        // Check for existing symbol
        if (input.GetSymbol(Name.Content) != null)
        {
            input.Logger.Error(Name, "This symbol already exists.");
            return;
        }

        // Add symbol
        input.Module.Symbols.Add(Name.Content, new Symbol(SymbolKind.Function, Property, input.Module, Name.Content));
    }

    // Resolve symbols
    public override void ResolveSymbols(Input input)
    {
        foreach (Node parameter in Parameters)
            parameter.ResolveSymbols(input);

        if (Body != null)
            Body.ResolveSymbols(input);
    }

    // Resolve types
    public override Type ResolveTypes(Input input)
    {
        Symbol symbol = input.Module.Symbols.GetValueOrDefault(Name.Content);

        TypeFunction type = new TypeFunction()
        {
            Return = Type.TypeCheck(input)
        };

        foreach (Node parameter in Parameters)
            type.Parameters.Add(parameter.ResolveTypes(input));

        symbol.Type = type;

        if (Body != null)
            Body.ResolveTypes(input);

        return null;
    }

    // Type checking
    public override Type TypeCheck(Input input)
    {
        input.CurrentFunction = this;

        if (Body != null)
            Body.TypeCheck(input);

        return null;
    }

    // Emit C++ code
    public override void Emit(Input input)
    {
        string mangle   = "";
        string head     = "";
        string property = "";

        // Mangle name and get property
        if (Property != PropertyKind.Extern && Name.Content != "main") 
        {
            mangle   = $"su_{input.Module.Name.Replace('.', '_')}_";
            property = "extern ";
        }
        else
            property = "extern \"C\" ";

        // Generate function head
        head += $"{Type.GetString}{mangle}{Name.Content}(";

        for (int index = 0; index < Parameters.Count; ++index)
        {
            head += Parameters[index].GetString;

            if (index != (Parameters.Count - 1))
                head += ", ";
        }

        head += ')';

        // Emit function declaration
        if (Name.Content != "main")
            input.Output.ExternalFunctionDeclarations += $"{property}{head};\n";

        if (Body != null)
        {
            // Emit function definition
            input.Output.FunctionDefinitions += head;

            // Emit function body
            Body.Emit(input);

            // Emit new line
            input.Output.FunctionDefinitions += '\n';
        }
    }
}