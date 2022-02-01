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
            string       mangle = "";
            string       name   = Name.GetString;
            Symbol       symbol = input.GetSymbol(name);
            NodeFunction node   = (NodeFunction)symbol.Node;

            // Mangle name
            if (node.Property != PropertyKind.Extern)
            {
                /*
                    Since the symbol property is not extern,
                    we need to add su_ and the module name if there is none.
                    Example:

                        symbol name: hello();
                        new    name: su_someModuleName_hello();
                */
                mangle = "su_"; 

                if (!name.Contains(symbol.Module.Name))
                    mangle += $"{symbol.Module.Name.Replace('.', '_')}_";
            }
            else
            {
                /*
                    Since the symbol property is extern,
                    we need to remove the module name if there is one.
                    Example:

                        symbol name: someModuleName.hello();
                        new    name: hello();
                */
                if (name.Contains(symbol.Module.Name))
                    name = name.Replace($"{symbol.Module.Name}.", "");
            }

            input.Output.FunctionDefinitions += $"{mangle}{name.Replace('.', '_')}(";

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