namespace Suteki.Compiler;

using System.Collections.Generic;

public class Input
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

    public Dictionary<string, Symbol> Locals;

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
        Locals          = new Dictionary<string, Symbol>();

        // Set scanner source
        Scanner.Set(source);
    }

    // Get symbol 
    public Symbol GetSymbol(string name, Token token = null)
    {
        // TODO: improve this
        Symbol foundSymbol = null;

        // Try finding symbol in local scope
        if (Locals.ContainsKey(name))
        {
            foundSymbol = Locals[name];

            if (!name.Contains('.'))
                goto End;
        }

        // Try finding symbol in current module
        if (Module.HasSymbol(name))
        {
            foundSymbol = Module.GetSymbol(name);

            if (!name.Contains('.'))
                goto End;
        }

        // Try finding symbol in imported modules
        foreach (Module module in Imports)
        {
            if (module.HasSymbol(name))
            {
                // Check for ambiguity
                if (foundSymbol != null && token != null)
                    Logger.Error(token, $"Ambiguous reference between '{foundSymbol.Module.Name}.{foundSymbol.Name}' and '{module.Name}.{name}'.");

                foundSymbol = module.GetSymbol(name);

                if (!name.Contains('.'))
                    goto End;
            }

            foreach (Module publicModule in module.Imports)
            {
                if (publicModule.HasSymbol(name))
                {
                    // Check for ambiguity
                    if (foundSymbol != null && token != null)
                        Logger.Error(token, $"Ambiguous reference between '{foundSymbol.Module.Name}.{foundSymbol.Name}' and '{publicModule.Name}.{name}'.");

                    foundSymbol = publicModule.GetSymbol(name);
                    
                    if (!name.Contains('.'))
                        goto End;
                }
            }
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
                if (Config.HasModule(nameSplitted) && (index + 1) < names.Length)
                {
                    Module module     = Config.GetModule(nameSplitted);
                    string symbolName = names[index + 1];

                    if (module.HasSymbol(symbolName))
                        lastSymbol = module.GetSymbol(symbolName);
                }
                
                // Check if module name is a module
                else if (Config.HasModule(moduleName) && (index + 1) < names.Length)
                {
                    Module module = Config.GetModule(moduleName);
                    
                    if ((index + 1) < names.Length)
                    {
                        string symbolName = names[index + 1];

                        if (module.HasSymbol(symbolName))
                            lastSymbol = module.GetSymbol(symbolName);
                    }
                }
            }

            // Check for ambiguity
            if (foundSymbol != null && token != null)
                Logger.Error(token, $"Ambiguous reference between '{foundSymbol.Module.Name}.{foundSymbol.Name}' and '{lastSymbol.Module.Name}.{lastSymbol.Name}'.");

            // Add import if needed
            if (foundSymbol != null && !Imports.Contains(foundSymbol.Module) &&
                foundSymbol.Module.Name != "global")
                Imports.Add(foundSymbol.Module);

            foundSymbol = lastSymbol;
        }

End:
        // Check if symbol is private
        if (foundSymbol != null && token != null &&
            foundSymbol.Property == PropertyKind.Private &&
            Module.Name != foundSymbol.Module.Name)
            Logger.Error(token, $"This symbol is private and can't be used outside the module '{foundSymbol.Module.Name}'.");

        return foundSymbol;
    }
}