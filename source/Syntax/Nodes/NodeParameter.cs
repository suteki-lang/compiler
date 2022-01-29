using System.Collections.Generic;

namespace Suteki
{
    class NodeParameter : Node
    {
        public Node  Type;
        public Token Name;

        public override string GetString => $"{Type.GetString}{Name.Content}";
        public override Token  GetToken  => Name;

        // Type checking
        public override ExpressionKind TypeCheck(Input input)
        {
            return Type.TypeCheck(input);
        }
    }
}