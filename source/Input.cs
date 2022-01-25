using System.Collections.Generic;

namespace Suteki
{
    class Input
    {
        public string       Path;
        public string       Source;
        public Module       Module;
        public NodeFunction CurrentFunction;
        public Output       Output;
        public Logger       Logger;
        public Scanner      Scanner;
        public List<Node>   Nodes;
        public List<Module> Imports;
        public bool         SymbolsRegistered;

        // Input Constructor
        public Input(string path, string source)
        {
            Output            = new Output();
            Logger            = new Logger();
            Scanner           = new Scanner();
            Nodes             = new List<Node>();
            Imports           = new List<Module>();
            
            Path              = path;
            Logger.Path       = path;
            Source            = source;
            Module            = null;
            SymbolsRegistered = false;

            Scanner.Set(source);
        }

        // Find symbol
        public bool Find(string name)
        {
            if (Module.Symbols.ContainsKey(name))
                return true;

            foreach (Module module in Imports)
            {
                if (module.Symbols.ContainsKey(name))
                    return true;
            }

            return false;
        }

        // Find symbol
        public bool Find(Token name)
        {
            return Find(name.Content);
        }

        // Get symbol 
        public Symbol GetSymbol(string name)
        {
            if (Module.Symbols.ContainsKey(name))
                return Module.Symbols[name];

            foreach (Module module in Imports)
            {
                if (module.Symbols.ContainsKey(name))
                    return module.Symbols[name];
            }

            return null;
        }

        // Get symbol
        public Symbol GetSymbol(Token name)
        {
            return GetSymbol(name.Content);
        }
    }
}