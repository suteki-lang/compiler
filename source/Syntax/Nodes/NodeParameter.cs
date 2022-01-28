using System.Collections.Generic;

namespace Suteki
{
    class NodeParameter : Node
    {
        public Node  Type;
        public Token Name;

        public override string GetString => $"{Type.GetString}{Name.Content}";
        public override Token  GetToken  => Name;
    }
}