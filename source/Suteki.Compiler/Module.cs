namespace Suteki.Compiler;

using System.Collections.Generic;

public class Module
{
    public string                     Name;
    public List<Module>               Imports;
    public Dictionary<string, Symbol> Symbols;

    // Constructor
    public Module(string name)
    {
        Name    = name;
        Imports = new List<Module>();
        Symbols = new Dictionary<string, Symbol>();
    }

    // Add symbol to module
    public void AddSymbol(string symbolName, Symbol symbol)
    {
        Symbols.Add(symbolName, symbol);
    }

    // Check if module has symbol
    public bool HasSymbol(string symbolName)
    {
        return Symbols.ContainsKey(symbolName);
    }

    // Get symbol from module
    public Symbol GetSymbol(string symbolName)
    {
        if (HasSymbol(symbolName))
            return Symbols[symbolName];

        return null;
    }
}