using System.Collections.Generic;

namespace Suteki
{
    class Module
    {
        public string                     Name;
        public Dictionary<string, Symbol> Symbols;

        // Constructor
        public Module(string name)
        {
            Name    = name;
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
}