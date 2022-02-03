using System.Collections.Generic;

namespace Suteki
{
    class NodeCall : Node
    {
        public Node       Name;
        public List<Node> Parameters = new List<Node>();

        public override Token GetToken => Name.GetToken;

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

            return null;
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

            input.Output.FunctionDefinitions += ");\n";
        }
    }
}