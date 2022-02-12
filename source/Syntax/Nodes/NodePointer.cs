namespace Suteki
{
    class NodePointer : Node
    {
        public Node PointsTo;
        
        public override Token GetToken => PointsTo.GetToken;

        public override string GetString
        {
            get
            {
                return $"{PointsTo.GetString}*";
            }
        }

        // Type checking
        public override Type TypeCheck(Input input)
        {
            return new TypePointer() 
            {
                Base = PointsTo.TypeCheck(input)
            };
        }
    }
}