namespace Suteki.Compiler;

using System.Collections.Generic;

public class NodeCall : Node
{
    public bool       IsExpression;
    public Node       Name;
    public List<Node> Parameters = new List<Node>();

    public override Token    GetToken => Name.GetToken;
    public override NodeKind Kind     => NodeKind.Call;

    public override string GetString
    {
        get 
        {
            string result = $"{Name.GetString}(";

            for (int index = 0; index < Parameters.Count; ++index)
            {
                result += Parameters[index].GetString;

                if (index != (Parameters.Count - 1))
                    result += ", ";
            }

            result += ')';

            if (!IsExpression)
                result += ";\n";

            return result;
        }
    }

    // Resolve symbols
    public override void ResolveSymbols(Input input)
    {
        if (input.GetSymbol(Name.GetString, GetToken) == null)
            input.Logger.Error(GetToken, "This symbol does not exists.");
    }

    // Type checking
    public override Type TypeCheck(Input input)
    {
        NodeFunction node = (NodeFunction)input.GetSymbol(Name.GetString).Node;

        // Check for parameter count
        if (Parameters.Count != node.Parameters.Count)
        {
            input.Logger.Error(Name.GetToken, "Function call parameter(s) count does not match function parameter(s) count.");
            return null;
        }

        // Compare parameter types
        for (int index = 0; index < node.Parameters.Count; ++index)
        {
            Type parameter  = node.Parameters[index].TypeCheck(input);
            Type expression = Parameters     [index].TypeCheck(input);

            if (!parameter.IsIdentical(expression))
                input.Logger.Error(Parameters[index].GetToken, $"Function call parameter ({index}) type does not match expression type.");
        }

        return node.Type.TypeCheck(input);
    }

    // Emit C++ code
    public override void Emit(Input input)
    {
        Symbol       symbol = input.GetSymbol(Name.GetString);
        NodeFunction node   = ((NodeFunction)symbol.Node);
        string       name   = Config.MangleName(Name.GetString, node.Property, symbol);

        input.Output.FunctionDefinitions += $"{name}(";

        for (int index = 0; index < Parameters.Count; ++index)
        {
            Parameters[index].Emit(input);

            if (index != (Parameters.Count - 1))
                input.Output.FunctionDefinitions += ", ";
        }

        input.Output.FunctionDefinitions += ')';

        if (!IsExpression)
            input.Output.FunctionDefinitions += ";\n";
    }
}