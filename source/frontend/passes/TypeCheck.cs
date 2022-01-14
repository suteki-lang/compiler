namespace Suteki
{
    partial class NodePrimitive : Node
    {
        public override ExpressionKind TypeCheck(Input input)
        {
            switch (Kind)
            {
                case PrimitiveKind.Void:
                    return ExpressionKind.Void;

                case PrimitiveKind.Bool:
                    return ExpressionKind.Bool;

                case PrimitiveKind.String:
                    return ExpressionKind.String;

                case PrimitiveKind.Single:
                case PrimitiveKind.Double:
                    return ExpressionKind.Float;

                default:
                    return ExpressionKind.Integer;
            }
        }
    }

    partial class NodeBool : Node
    {
        public override ExpressionKind TypeCheck(Input input)
        {
            return ExpressionKind.Bool;
        }
    }

    partial class NodeInteger : Node
    {
        public override ExpressionKind TypeCheck(Input input)
        {
            return ExpressionKind.Integer;
        }
    }

    partial class NodeFloat : Node
    {
        public override ExpressionKind TypeCheck(Input input)
        {
            return ExpressionKind.Float;
        }
    }

    partial class NodeString : Node
    {
        public override ExpressionKind TypeCheck(Input input)
        {
            return ExpressionKind.String;
        }
    }

    partial class NodeFunction : Node
    {
        public override ExpressionKind TypeCheck(Input input)
        {
            if (Property != PropertyKind.Extern)
            {
                input.CurrentFunction = this;
                Block.TypeCheck(input);
            }
            
            return ExpressionKind.Void;
        }
    }

    partial class NodeParameter : Node
    {
        public override ExpressionKind TypeCheck(Input input)
        {
            return Type.TypeCheck(input);
        }
    }

    partial class NodeBlock : Node
    {
        public override ExpressionKind TypeCheck(Input input)
        {
            foreach (Node node in Statements)
                node.TypeCheck(input);

            return ExpressionKind.Void;
        }
    }

    partial class NodeReturn : Node
    {
        public override ExpressionKind TypeCheck(Input input)
        {
            ExpressionKind expressionType = (Expression == null) ?
                                             ExpressionKind.Void :
                                             Expression.TypeCheck(input);
            ExpressionKind functionType   = input.CurrentFunction.Type.TypeCheck(input);

            // Compare types
            if (!Type.Compare(functionType, expressionType))
                input.Logger.Error(Start, "Return value type does not match function return type.");
    
            return ExpressionKind.Void;
        }
    }
}