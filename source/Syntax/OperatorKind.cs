namespace Suteki
{
    enum OperatorKind
    {
        None,
        Add,
        Subtract,
        Divide,
        Multiply,
        Equality,
    }

    class Operator
    {
        public static string ToString(OperatorKind kind)
        {
            switch (kind)
            {
                case OperatorKind.Add:
                    return "+";

                case OperatorKind.Subtract:
                    return "-";

                case OperatorKind.Divide:
                    return "/";

                case OperatorKind.Multiply:
                    return "*";

                case OperatorKind.Equality:
                    return "==";

                default:
                    return "<?>";
            }
        }
    }
}