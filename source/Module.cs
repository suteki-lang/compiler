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
    }
}