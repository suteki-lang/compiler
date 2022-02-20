namespace Suteki;

class NodeIf : Node
{
    public Token Token;
    public Node  Condition;
    public Node  ThenBody;
    public Node  ElseBody;

    public override Token    GetToken => Token;
    public override NodeKind Kind      => NodeKind.If;

    // Resolve symbols
    public override void ResolveSymbols(Input input)
    {
        Condition.ResolveSymbols(input);
        ThenBody .ResolveSymbols(input);

        if (ElseBody != null)
            ElseBody.ResolveSymbols(input);
    }

    // Type checking
    public override Type TypeCheck(Input input)
    {
        Type conditionType = Condition.TypeCheck(input);

        if (!conditionType.IsBool())
            input.Logger.Error(Condition.GetToken, "Expected boolean, integer or pointer condition.");

        return null;
    }

    // Emit C++ code
    public override void Emit(Input input)
    {
        input.Output.FunctionDefinitions += "if (";
        Condition.Emit(input);
        input.Output.FunctionDefinitions += ")";

        if (ThenBody.Kind != NodeKind.Block)
        {
            input.Output.FunctionDefinitions += '\n';
            
            ++input.Output.Tabs;
            input.Output.FunctionDefinitions += input.Output.GetTabs();
            
            ThenBody.Emit(input);
            --input.Output.Tabs;
        }
        else
            ThenBody.Emit(input);

        if (ElseBody != null)
        {   
            input.Output.FunctionDefinitions += input.Output.GetTabs();
            input.Output.FunctionDefinitions += "else ";

            if (ElseBody.Kind != NodeKind.If && ElseBody.Kind != NodeKind.Block)
            {
                input.Output.FunctionDefinitions += '\n';

                ++input.Output.Tabs;
                input.Output.FunctionDefinitions += input.Output.GetTabs();
                
                ElseBody.Emit(input);
                --input.Output.Tabs;
            }
            else
                ElseBody.Emit(input);
        }
    }
}