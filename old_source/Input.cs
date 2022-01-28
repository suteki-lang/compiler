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
        public List<string> Includeds;
        public bool         SymbolsRegistered;

        // Input Constructor
        public Input(string path, string source)
        {
            Output            = new Output();
            Logger            = new Logger();
            Scanner           = new Scanner();
            Nodes             = new List<Node>();
            Imports           = new List<Module>();
            Includeds         = new List<string>();
            
            Path              = path;
            Logger.Path       = path;
            Source            = source;
            Module            = null;
            SymbolsRegistered = false;

            Scanner.Set(source);
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

        // Check for name
        private Symbol CheckName(string name)
        {
            foreach (KeyValuePair<string, Module> module in Config.Modules)
            {
                if (module.Key == name)
                {
                    // Emit include
                    if (!Includeds.Contains(module.Key))
                    {
                        Output.Includes += $"#include <modules/{module.Key}.hpp>\n";
                        Includeds.Add(module.Key);
                    }

                    return new Symbol(PropertyKind.None, SymbolKind.None, null);
                }

                if (module.Value.Symbols.ContainsKey(name))
                    return module.Value.Symbols[name];
            }

            return null;
        }

        // Get symbol
        public Symbol GetSymbol(Node name)
        {
            Symbol lastSymbol = null;
            string realName   = name.AsString();

            if (!realName.Contains('.'))
                return GetSymbol(realName);
            else
            {
                string[] names = realName.Split('.');

                foreach (string checkName in names)
                    lastSymbol = CheckName(checkName);
            }

            return lastSymbol;
        }
    }
}