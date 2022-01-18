using System.Collections.Generic;

namespace Suteki
{
    class Input
    {
        public string       Path;
        public string       Source;
        public string       Module               = "";
        public NodeFunction CurrentFunction      = null;
        public Output       Output               = new Output();
        public Logger       Logger               = new Logger();
        public Scanner      Scanner              = new Scanner();
        public List<Node>   Nodes                = new List<Node>();
        public bool         SymbolsAreRegistered = false;

        public Dictionary<string, Symbol> Globals = new Dictionary<string, Symbol>();

        // Input Constructor
        public Input(string path, string source)
        {
            Path        = path;
            Logger.Path = path;
            Source      = source;

            Scanner.Set(source);
        }

        // Get symbol 
        public Symbol GetSymbol(string name)
        {
            return Globals[name];
        }

        // Get symbol
        public Symbol GetSymbol(Token name)
        {
            return Globals[name.Content];
        }
    }
}