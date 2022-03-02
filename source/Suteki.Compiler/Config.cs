namespace Suteki.Compiler;

using System.Collections.Generic;

public class Config
{
    public static bool         CanLog      = true;
    public static bool         HadError    = false;
    public static int          PointerSize = 8;
    public static string       OutputPath  = "";
    public static string       RuntimePath = "";
    public static List<string> Versions    = new List<string>() { "OS.Linux" };
    public static List<Input>  Inputs      = new List<Input>();

    public static Dictionary<string, Module> Modules = new Dictionary<string, Module>();

    // Mangle name
    public static string MangleName(string name, PropertyKind property, Symbol symbol)
    {
        string result = name.Replace('.', '_');

        if (property == PropertyKind.Extern)
        {
            // Remove module name 
            if (name.Contains(symbol.Module.Name))
                result = result.Replace($"{symbol.Module.Name}_", "");
        }
        else
        {
            // Add 'su_' and module name
            string mangle = "su_"; 

            if (!name.Contains('.'))
                mangle += $"{symbol.Module.Name.Replace('.', '_')}_";

            result = $"{mangle}{result}";
        }

        return result;
    }
}