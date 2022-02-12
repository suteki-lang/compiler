namespace Suteki
{
    class NodeBinary : Node
    {
        Node         Left;
        Node         Right;
        OperatorKind Op;

        // Constructor
        public NodeBinary(Node left, OperatorKind op, Node right)
        {
            Left  = left;
            Op    = op;
            Right = right;
        }

        public override string GetString => $"{Left.GetString} {Operator.ToString(Op)} {Right.GetString}";
        public override Token  GetToken  => Token.From(Left.GetToken, GetString);

        // Type checking
        public override Type TypeCheck(Input input)
        {
            Type left  = Left.TypeCheck(input);
            Type right = Right.TypeCheck(input);

            if (!left.IsIdentical(right, true))
                input.Logger.Error(GetToken, "Expression types does not match.");

            if (left.IsFloat())
                return left;

            return right;
        }

        // Emit C++ code
        public override void Emit(Input input)
        {
            input.Output.FunctionDefinitions += GetString;
        }
    }
}