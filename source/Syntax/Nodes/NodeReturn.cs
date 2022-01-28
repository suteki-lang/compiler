namespace Suteki
{
    class NodeReturn : Node
    {
        public Token Token;
        public Node  Expression;

        public override Token GetToken => Token;

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