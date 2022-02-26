namespace Suteki.Compiler;

using System.Collections.Generic;

public class NodeBlock : Node
{
    public Token      Token;
    public List<Node> Statements = new List<Node>();

    public override Token    GetToken => Token;
    public override NodeKind Kind     => NodeKind.Block;

    // Resolve symbols
    public override void ResolveSymbols(Input input)
    {
        foreach (Node node in Statements)
            node.ResolveSymbols(input);
    }

    // Type checking
    public override Type TypeCheck(Input input)
    {
        foreach (Node node in Statements)
            node.TypeCheck(input);

        return null;
    }
    
    // Emit C++ code
    public override void Emit(Input input)
    {
        input.Output.FunctionDefinitions += '\n';
        input.Output.FunctionDefinitions += input.Output.GetTabs();
        input.Output.FunctionDefinitions += "{\n";

        if (Statements.Count == 0)
        {
            input.Output.FunctionDefinitions += "\t\n";
            input.Output.FunctionDefinitions += input.Output.GetTabs();
            input.Output.FunctionDefinitions += "}\n";
            return;
        }

        ++input.Output.Tabs;

        foreach (Node node in Statements)
        {
            input.Output.FunctionDefinitions += input.Output.GetTabs();
            node.Emit(input);
        }

        --input.Output.Tabs;
        input.Output.FunctionDefinitions += input.Output.GetTabs();
        input.Output.FunctionDefinitions += "}\n";
    }
}