namespace Suteki
{
    class NodeUnary : Node
    {
        OperatorKind Op;
        Node         Operand;

        // Constructor
        public NodeUnary(OperatorKind op, Node operand)
        {
            Op      = op;
            Operand = operand;
        }

        public override string GetString => $"{Operator.ToString(Op)}{Operand.GetString}";
        public override Token  GetToken  => Token.From(Operand.GetToken, GetString);

        // Type checking
        public override Type TypeCheck(Input input)
        {
            Type type  = Operand.TypeCheck(input);
            Type check = type;

            if (check.IsConst())
                check = type.GetDeconstedType();

            if (!check.IsBool() && !check.IsInteger() && !check.IsFloat())
                input.Logger.Error(GetToken, "Invalid unary expression.");

            return type;
        }

        // Emit C++ code
        public override void Emit(Input input)
        {
            input.Output.FunctionDefinitions += GetString;
        }
    }
}