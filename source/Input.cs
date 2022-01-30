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
        public List<Module> Imports;

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
            Imports         = new List<Module>();

            // Set scanner source
            Scanner.Set(source);
        }

        // Check for symbol
        private Symbol CheckName(string name)
        {
            foreach (KeyValuePair<string, Module> module in Config.Modules)
            {
                if (module.Value.Symbols.ContainsKey(name))
                {
                    // Add import
                    if (!Imports.Contains(module.Value))
                        Imports.Add(module.Value);

                    return module.Value.Symbols[name];
                }
            }
            
            return null;
        }

        // Get symbol 
        public Symbol GetSymbol(string name)
        {
            // Try finding symbol in current module
            if (Module.Symbols.ContainsKey(name))
                return Module.Symbols[name];

            // Try finding symbol in imported modules
            foreach (Module module in Imports)
            {
                if (module.Symbols.ContainsKey(name))
                    return module.Symbols[name];
            }

            // Try finding symbol in global module
            if (Config.Modules.ContainsKey("global") && 
                Config.Modules["global"].Symbols.ContainsKey(name))
                return Config.Modules["global"].Symbols[name];

            // Try finding symbol from a module that is not imported
            if (name.Contains('.'))
            {
                Symbol   lastSymbol = null;
                string[] names      = name.Split('.');

                foreach (string checkName in names)
                    lastSymbol = CheckName(checkName);

                return lastSymbol;
            }

            return null;
        }
    }
}