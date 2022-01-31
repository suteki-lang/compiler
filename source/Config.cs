using System.Collections.Generic;

namespace Suteki
{
    class Config
    {
        public static bool        HadError               = false;
        public static string      OutputPath             = "../tests/out/";
        public static List<Input> Inputs                 = new List<Input>();
        public static Dictionary<string, Module> Modules = new Dictionary<string, Module>();
    
        // Add module
        public static void AddModule(string moduleName, Module module)
        {
            Modules.Add(moduleName, module);
        }

        // Check if module exists
        public static bool HasModule(string moduleName)
        {
            return Modules.ContainsKey(moduleName);
        }

        // Get module
        public static Module GetModule(string moduleName)
        {
            if (HasModule(moduleName))
                return Modules[moduleName];

            return null;
        } 

        // Add symbol to module
        public static void AddSymbol(string moduleName, string symbolName, Symbol symbol)
        {
            GetModule(moduleName).AddSymbol(symbolName, symbol);
        }

        // Check if module has symbol
        public static bool HasSymbol(string moduleName, string symbolName)
        {
            return (HasModule(moduleName) && Modules[moduleName].HasSymbol(symbolName));
        }

        // Get symbol from module
        public static Symbol GetSymbol(string moduleName, string symbolName)
        {
            if (HasSymbol(moduleName, symbolName))
                return Modules[moduleName].GetSymbol(symbolName);

            return null;
        }
    }
}