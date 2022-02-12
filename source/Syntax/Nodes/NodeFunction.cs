using System.Collections.Generic;

namespace Suteki
{
    class NodeFunction : Node
    {
        public PropertyKind Property;
        public Node         Type;
        public Token        Name;
        public List<Node>   Parameters = new List<Node>();
        public Node         Block;

        public override Token GetToken => Name;

        // Register symbols
        public override void RegisterSymbols(Input input)
        {
            // Check for existing symbol
            if (input.GetSymbol(Name.Content) != null)
            {
                input.Logger.Error(Name, "This symbol was already declared.");
                return;
            }

            // Add symbol
            input.Module.AddSymbol(Name.Content, new Symbol(SymbolKind.Function, input.Module, Name.Content, this));
        }

        // Resolve symbols
        public override void ResolveSymbols(Input input)
        {
            foreach (Node parameter in Parameters)
                parameter.ResolveSymbols(input);

            if (Block != null)
                Block.ResolveSymbols(input);
        }

        // Type checking
        public override Type TypeCheck(Input input)
        {
            input.CurrentFunction = this;

            if (Block != null)
                Block.TypeCheck(input);

            return null;
        }

        // Emit C++ code
        public override void Emit(Input input)
        {
            string mangle   = "";
            string head     = "";
            string property = "";

            // Mangle name and get property
            if (Property != PropertyKind.Extern) 
            {
                mangle   = $"su_{input.Module.Name.Replace('.', '_')}_"; 
                property = "extern \"C\" "; //"static ";
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
            // if (Property == PropertyKind.Extern)
                input.Output.ExternalFunctionDeclarations += $"{property}{head};\n";
            // else
                // input.Output.FunctionDeclarations += $"{property}{head};\n";

            if (Block != null)
            {
                // Emit function definition
                input.Output.FunctionDefinitions += head;

                // Emit function block
                Block.Emit(input);

                // Emit new line
                input.Output.FunctionDefinitions += '\n';
            }
        }
    }
}