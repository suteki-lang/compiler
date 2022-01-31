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

        // Get symbol 
        public Symbol GetSymbol(string name, Token token = null)
        {
            Symbol foundSymbol = null;

            // Try finding symbol in current module
            if (Module.HasSymbol(name))
                foundSymbol = Module.GetSymbol(name);

            // Try finding symbol in imported modules
            foreach (Module module in Imports)
            {
                if (module.HasSymbol(name))
                {
                    // Check for ambiguity
                    if (foundSymbol != null && token != null)
                        Logger.Error(token, $"Ambiguous reference between '{foundSymbol.Module.Name}.{foundSymbol.Name}' and '{module.Name}.{name}'.");

                    foundSymbol = module.GetSymbol(name);
                }
            }

            // Try finding symbol in global module
            if (Config.HasSymbol("global", name))
            {
                // Check for ambiguity
                if (foundSymbol != null && token != null)
                    Logger.Error(token, $"Ambiguous reference between '{foundSymbol.Module.Name}.{foundSymbol.Name}' and 'global.{name}'.");

                foundSymbol = Config.GetSymbol("global", name);
            }

            // Try finding symbol from a module that is not imported
            if (name.Contains('.'))
            {
                string[] names      = name.Split('.');
                string   moduleName = "";
                Symbol   lastSymbol = null;

                for (int index = 0; index < names.Length; ++index)
                {
                    string nameSplitted = names[index];

                    // Add splitted name to module name
                    if (moduleName != "")
                        moduleName += '.'; 
                        
                    moduleName += nameSplitted;

                    // Check if splitted name is a module
                    if (Config.HasModule(nameSplitted))
                    {
                        Module module     = Config.GetModule(nameSplitted);
                        string symbolName = names[index + 1];

                        if (module.HasSymbol(symbolName))
                            lastSymbol = module.GetSymbol(symbolName);
                    }
                    
                    // Check if module name is a module
                    else if (Config.HasModule(moduleName))
                    {
                        Module module = Config.GetModule(moduleName);
                        
                        if ((index + 1) < names.Length)
                        {
                            string symbolName = names[index + 1];

                            if (module.HasSymbol(symbolName))
                                lastSymbol = module.GetSymbol(symbolName);
                        }
                    }
                     
                    // Check if splitted name is a global symbol
                    else if (Config.HasSymbol("global", nameSplitted))
                        lastSymbol = Config.GetSymbol("global", nameSplitted);
                }

                // Check for ambiguity
                if (foundSymbol != null && token != null)
                    Logger.Error(token, $"Ambiguous reference between '{foundSymbol.Module.Name}.{foundSymbol.Name}' and '{lastSymbol.Module.Name}.{lastSymbol.Name}'.");

                // Add import
                if (!Imports.Contains(lastSymbol.Module))
                    Imports.Add(lastSymbol.Module);

                foundSymbol = lastSymbol;
            }

            return foundSymbol;
        }
    }
}