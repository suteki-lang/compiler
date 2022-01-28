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

        // Emit C++ code
        public override void Emit(Input input)
        {
            string mangle   = "";
            string head     = "";
            string property = "";

            // Mangle name and get property
            if (Property != PropertyKind.Extern) 
            {
                mangle   = "su_"; 
                property = "static ";
            }
            else
                property = "extern \"C\" ";

            // Generate function head
            head += $"{Type.GetString}{mangle}{Name.Content}(";

            for (int index = 0; index < Parameters.Count; ++index)
            {
                head += Parameters[index].GetString;

                if (index < (Parameters.Count - 1))
                    head += ", ";
            }

            head += ')';

            // Emit function declaration
            if (Property == PropertyKind.Extern)
                input.Output.ExternalFunctionDeclarations += $"{property}{head};\n";
            else
                input.Output.FunctionDeclarations += $"{property}{head};\n";

            // Emit function definition
            input.Output.FunctionDefinitions += head;

            // Emit function block
            if (Block != null)
                Block.Emit(input);

            // Emit new line
            input.Output.FunctionDefinitions += '\n';
        }
    }
}