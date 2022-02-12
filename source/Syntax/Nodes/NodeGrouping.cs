namespace Suteki
{
    class NodeGrouping : Node
    {
        Node Expression;

        // Constructor
        public NodeGrouping(Node expression)
        {
            Expression = expression;
        }

        public override string GetString => $"({Expression.GetString})";
        public override Token  GetToken  => Token.From(Expression.GetToken, GetString);

        // Type checking
        public override Type TypeCheck(Input input)
        {
            return Expression.TypeCheck(input);
        }

        // Emit C++ code
        public override void Emit(Input input)
        {
            input.Output.FunctionDefinitions += GetString;
        }
    }
}