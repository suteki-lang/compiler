namespace Suteki
{
    class NodeReturn : Node
    {
        public Token Token;
        public Node  Expression;

        public override Token GetToken => Token;

        // Type checking
        public override ExpressionKind TypeCheck(Input input)
        {
            ExpressionKind expressionType = (Expression == null) ?
                                             ExpressionKind.Void :
                                             Expression.TypeCheck(input);
            ExpressionKind functionType   = input.CurrentFunction.Type.TypeCheck(input);

            // Compare types
            if (!Type.Compare(functionType, expressionType))
                input.Logger.Error(GetToken, "Return value type does not match function return type.");
    
            return ExpressionKind.Void;
        }

        // Emit C++ code
        public override void Emit(Input input)
        {
            input.Output.FunctionDefinitions += "return";

            if (Expression != null)
            {
                input.Output.FunctionDefinitions += ' ';
                Expression.Emit(input);
            }

            input.Output.FunctionDefinitions += ";\n";
        }
    }
}