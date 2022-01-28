using System.Collections.Generic;

namespace Suteki
{
    class Input
    {
        public string     Path;
        public string     Source;
        public Output     Output;
        public List<Node> Nodes;

        // Constructor
        public Input()
        {
            Path   = "";
            Source = "";
            Output = new Output();
            Nodes  = new List<Node>();
        }
    }
}