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
}