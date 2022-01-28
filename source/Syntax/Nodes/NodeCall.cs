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
            if (input.GetSymbol(Name.GetString) == null)
                input.Logger.Error(GetToken, "This symbol does not exists.");
        }

        // Emit C++ code
        public override void Emit(Input input)
        {
            string       mangle = "";
            string       name   = Name.GetString;
            Symbol       symbol = input.GetSymbol(name);
            NodeFunction node   = (NodeFunction)symbol.Node;

            // Mangle name
            // NOTE: This doesn't look right
            if (node.Property != PropertyKind.Extern)
            {
                mangle = "su_"; 

                if (!name.Contains(symbol.Module.Name))
                    mangle += $"{symbol.Module.Name.Replace('.', '_')}_";
            }
            else
            {
                System.Console.WriteLine(name);

                System.Console.WriteLine(symbol.Module.Name);
                System.Console.WriteLine(input.Module.Name);
                if (name.Contains(input.Module.Name))
                    name = name.Replace(input.Module.Name + '.', "");

                System.Console.WriteLine(name);

                if (name.Contains(symbol.Module.Name))
                    name = name.Replace(symbol.Module.Name + '.', "");
                System.Console.WriteLine(name);
            }

            input.Output.FunctionDefinitions += $"{mangle}{name.Replace('.', '_')}(";

            for (int index = 0; index < Parameters.Count; ++index)
            {
                Parameters[index].Emit(input);

                if (index < (Parameters.Count - 1))
                    input.Output.FunctionDefinitions += ", ";
            }

            input.Output.FunctionDefinitions += ");\n";
        }
    }
}