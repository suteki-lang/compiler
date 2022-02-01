namespace Suteki
{
    class NodeReturn : Node
    {
        public Token Token;
        public Node  Expression;

        public override Token GetToken => Token;

        // Type checking
        public override Type TypeCheck(Input input)
        {
            Type expression;
            Type function = input.CurrentFunction.Type.TypeCheck(input);

            if (Expression == null)
                expression = new TypePrimitive() { Kind = PrimitiveKind.Void };
            else
                expression = Expression.TypeCheck(input);

            if (!function.IsIdentical(expression))
                input.Logger.Error(Expression.GetToken, "Return value type does not match function return type.");
    
            return null;
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