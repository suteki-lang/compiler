namespace Suteki
{
    class NodePointer : Node
    {
        public Node PointsTo;

        public override bool  IsPointer => true;
        public override Token GetToken  => PointsTo.GetToken;

        public override string GetString
        {
            get
            {
                return $"{PointsTo.GetString}*";
            }
        }

        // Type checking
        public override ExpressionKind TypeCheck(Input input)
        {
            return PointsTo.TypeCheck(input);
        }
    }
}