namespace Suteki
{
    partial class NodeImport : Node
    {
        public override void Emit(Input input)
        {
            // Get module path
            Input  module = Config.Inputs.Find((m) => m.Module == ModuleName);
            string path   = $"user/{module.Path.Replace(".su", "")}.hpp";

            // Emit include
            input.Output.Includes += $"#include <{path}>\n";
        }
    }

    partial class NodePrimitive : Node
    {
        public override string ToString()
        {
            // NOTE (ryaangu): do this better?
            string[] cTypes =
            {
                "void", 
                "bool", 
                "std::string",

                "unsigned char",
                "unsigned short",
                "unsigned int", 
                "unsigned long",

                "char",  
                "short", 
                "int",   
                "long",  

                "float",
                "double",
            };
            
            return cTypes[(int)Kind];
        }
    }

    partial class NodeBool : Node
    {
        public override void Emit(Input input)
        {
            input.Output.Source += Value.Data.ToString();
        }
    }

    partial class NodeInteger : Node
    {
        public override void Emit(Input input)
        {
            input.Output.Source += Value.Data.ToString();
        }
    }

    partial class NodeFloat : Node
    {
        public override void Emit(Input input)
        {
            input.Output.Source += Value.Data.ToString();
        }
    }

    partial class NodeString : Node
    {
        public override void Emit(Input input)
        {
            input.Output.Source += Value.Data.ToString();
        }
    }

    partial class NodeFunction : Node
    {
        public override void Emit(Input input)
        {
            // Generate function head
            string head  = $"{Type.ToString()} ";
                   head += $"{Name.Data.ToString()}";
                   head += '(';

            for (int index = 0; index < Parameters.Count; ++index)
            {
                head += Parameters[index].ToString();

                if (index < (Parameters.Count - 1))
                    head += ", ";
            }

            head += ')';

            // Emit header
            string cExtern = (Property == PropertyKind.Extern) ? "\"C\" "
                                                               : "";

            input.Output.Header += $"extern {cExtern}{head};\n";

            // Emit source
            if (Property != PropertyKind.Extern)
            {
                input.Output.Source += $"{head}\t\n";
                input.Output.Source += "{\n";
                Block.Emit(input);
                input.Output.Source += "}\n";
            }
        }
    }

    partial class NodeParameter : Node
    {
        public override string ToString()
        {
            return $"{Type.ToString()} {Name.Data.ToString()}";
        }
    }

    partial class NodeBlock : Node
    {
        public override void Emit(Input input)
        {
            if (Statements.Count == 0)
            {
                input.Output.Source += "\t\n";
                return;
            }

            input.Output.WriteTabs();
            input.Output.BeginScope();

            foreach (Node node in Statements)
            {
                input.Output.WriteTabs();
                node.Emit(input);
            }

            input.Output.EndScope();
        }
    }

    partial class NodeCall : Node
    {
        public override void Emit(Input input)
        {
            input.Output.Source += $"{Name.Data.ToString()}";
            input.Output.Source += '(';

            for (int index = 0; index < Parameters.Count; ++index)
            {
                Parameters[index].Emit(input);

                if (index < (Parameters.Count - 1))
                    input.Output.Source += ", ";
            }

            input.Output.Source += ");\n";
        }
    }

    partial class NodeReturn : Node
    {
        public override void Emit(Input input)
        {
            input.Output.Source += "return";

            if (Expression != null)
            {
                input.Output.Source += ' ';
                Expression.Emit(input);
            }

            input.Output.Source += ";\n";
        }
    }
}