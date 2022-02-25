namespace Suteki.Compiler;

using System;
using System.IO;
using System.Collections.Generic;
using Suteki.Utilities;

public class Linker
{
    // Get path
    public static string GetPath(string path)
    {
        string name = Path.GetFileName(path);

        // Format the path
        path = path.Replace("../", "").Replace(name, "");
        path = ("files/" + path);

        // Make the directory
        string directory = (Config.OutputPath + path);

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        return (path + name.Replace(".su", ""));            
    }

    // Start linking
    public static void Start()
    {
        // Check for valid paths
        if (!Config.OutputPath.EndsWith('/'))
            Config.OutputPath += '/';

        if (!Config.RuntimePath.EndsWith('/'))
            Config.RuntimePath += '/';

        // Check for paths
        if (!Directory.Exists(Config.OutputPath))
        {
            Console.Error.WriteLine(ConsoleColor.Red,    "Linker Error: ");
            Console.Error.WriteLine(ConsoleColor.White, $"The output path '{Config.OutputPath}' does not exists.");
            return;
        }

        if (!Directory.Exists(Config.RuntimePath))
        {
            Console.Error.WriteLine(ConsoleColor.Red,    "Linker Error: ");
            Console.Error.WriteLine(ConsoleColor.White, $"The runtime path '{Config.RuntimePath}' does not exists.");
            return;
        }

        // Clear output path
        Directory.Delete(Config.OutputPath, true);
        Directory.CreateDirectory(Config.OutputPath);

        // Write runtime folder
        string runtimeDirectory = $"{Config.OutputPath}runtime";
        Directory.CreateDirectory(runtimeDirectory);

        foreach (string file in Directory.GetFiles(Config.RuntimePath))
        {
            string fileName = Path.GetFileName(file);
            File.Copy(file, Path.Combine(runtimeDirectory, fileName), true);
        }

        // Write modules folder
        string modulesDirectory = $"{Config.OutputPath}modules";
        Directory.CreateDirectory(modulesDirectory);

        foreach (KeyValuePair<string, Module> pair in Config.Modules)
        {
            string output          = "";
            string moduleGuardName = $"MODULES_{pair.Key.ToUpper().Replace(".", "__")}_HPP";

            // Write guard
            output += $"#ifndef {moduleGuardName}\n";
            output += $"#define {moduleGuardName}\n\n";

            // Include public imports
            foreach (Module module in pair.Value.Imports)
                output += $"#include <modules/{module.Name}.hpp>\n";

            // Write spacing
            if (pair.Value.Imports.Count != 0)
                output += '\n';

            // Include files that uses this module
            foreach (Input input in Config.Inputs)
            {
                if (input.Module.Name == pair.Key)
                    output += $"#include <{GetPath(input.Path)}.hpp>\n";
            }

            // End guard
            output += "\n#endif";

            // Write
            File.WriteAllText($"{modulesDirectory}/{pair.Key}.hpp", output);
        }

        // Write files folder
        foreach (Input input in Config.Inputs)
        {
            string path       = GetPath(input.Path);
            string outputPath = Config.OutputPath + path;
            string guardName  = $"{path.Replace("/", "_").Replace(".", "__").ToUpper()}_HPP";

            string header = "";
            string source = "";

            bool sourceIsEmpty = true;

            // Header: Write guard
            header += $"#ifndef {guardName}\n";
            header += $"#define {guardName}\n\n";

            // Header: Include runtime
            header += "#include <runtime/runtime.hpp>\n\n";
            
            // Header: Write external function declarations
            header += input.Output.ExternalFunctionDeclarations;

            // Header: End guard
            if (input.Output.ExternalFunctionDeclarations != "")
                header += '\n';
                
            header += "#endif";

            // Source: Add includes
            source += $"#include <modules/{input.Module.Name}.hpp>\n";

            foreach (Module module in input.Imports)
                source += $"#include <modules/{module.Name}.hpp>\n";

            source += '\n';

            if (input.Output.FunctionDefinitions != "")
            {
                source        += input.Output.FunctionDefinitions.Substring(0, input.Output.FunctionDefinitions.Length - 2);
                sourceIsEmpty  = false;
            }

            // Write files
            File.WriteAllText($"{outputPath}.hpp", header);

            if (!sourceIsEmpty)
                File.WriteAllText($"{outputPath}.cpp", source);
        }
    }
}