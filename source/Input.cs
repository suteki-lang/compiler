using System.Collections.Generic;

namespace Suteki
{
    class Input
    {
        public string       Path;
        public string       Source;
        public Module       Module;
        public Output       Output;
        public NodeFunction CurrentFunction;
        public Logger       Logger;
        public Scanner      Scanner;
        public List<Node>   Nodes;

        // Constructor
        public Input(string path, string source)
        {
            Path            = path;
            Source          = source;
            Module          = null;
            Output          = new Output();
            CurrentFunction = null;
            Logger          = new Logger() { Path = path };
            Scanner         = new Scanner();
            Nodes           = new List<Node>();

            // Set scanner source
            Scanner.Set(source);
        }
    }
}