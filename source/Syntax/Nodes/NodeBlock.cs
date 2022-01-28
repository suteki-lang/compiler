using System.Collections.Generic;

namespace Suteki
{
    class NodeBlock : Node
    {
        public Token      Token;
        public List<Node> Statements = new List<Node>();

        public override Token GetToken => Token;
        
        // Emit C++ code
        public override void Emit(Input input)
        {
            input.Output.FunctionDefinitions += "\n{\n";

            if (Statements.Count == 0)
            {
                input.Output.FunctionDefinitions += "\t\n";
                input.Output.FunctionDefinitions += "}\n";
                return;
            }

            input.Output.FunctionDefinitions += input.Output.GetTabs();
            ++input.Output.Tabs;

            foreach (Node node in Statements)
            {
                input.Output.FunctionDefinitions += input.Output.GetTabs();
                node.Emit(input);
            }

            --input.Output.Tabs;
            input.Output.FunctionDefinitions += "}\n";
        }
    }
}