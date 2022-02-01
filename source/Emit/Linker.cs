using System.IO;
using System.Collections.Generic;

namespace Suteki
{
    struct Linker
    {
        // Get path
        private static string GetPath(string path)
        {
            string name = Path.GetFileName(path).Replace(".su", "");

            // Format the path
            path = path.Replace(".su", "").Replace("../", "").Replace(name, "");
            path = ("files/" + path);

            // Make the directory
            string directory = (Config.OutputPath + path);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return (path + name);            
        }

        // Start linking
        public static void Start()
        {
            // Clear output path
            Directory.Delete(Config.OutputPath, true);
            Directory.CreateDirectory(Config.OutputPath);

            // Write modules
            string modulesDirectory = $"{Config.OutputPath}modules";

            if (!Directory.Exists(modulesDirectory))
                Directory.CreateDirectory(modulesDirectory);

            foreach (KeyValuePair<string, Module> module in Config.Modules)
            {
                string output          = "";
                string moduleGuardName = $"MODULES_{module.Key.ToUpper().Replace(".", "__")}_HPP";

                // Write guard
                output += $"#ifndef {moduleGuardName}\n";
                output += $"#define {moduleGuardName}\n\n";

                // Add string type to global module
                // TODO: do a better way, like suteki folder with some runtime cpp files
                if (module.Key == "global")
                {
                    output += "struct string\n{\n\tstring(const char *) {}\n};\n\n";
                }

                foreach (Input input in Config.Inputs)
                {
                    if (input.Module.Name == module.Key)
                        output += $"#include <{GetPath(input.Path)}.hpp>\n";
                }

                // End guard
                output += "\n#endif";

                // Write
                File.WriteAllText($"{modulesDirectory}/{module.Key}.hpp", output);
            }

            // Write files
            foreach (Input input in Config.Inputs)
            {
                string path       = GetPath(input.Path);
                string outputPath = Config.OutputPath + path;
                string guardName  = $"{path.Replace("/", "_").Replace(".", "__").ToUpper()}_HPP";

                string header = "";
                string source = "";

                // Header: Write guard
                header += $"#ifndef {guardName}\n";
                header += $"#define {guardName}\n\n";
                header += input.Output.Includes;
                
                if (input.Output.ExternalFunctionDeclarations == "")
                    header += '\n';

                header += input.Output.ExternalFunctionDeclarations;

                // Header: End guard
                header += "\n#endif";

                // Source: Add includes
                source += $"#include <modules/{input.Module.Name}.hpp>\n";

                foreach (Module module in input.Imports)
                    source += $"#include <modules/{module.Name}.hpp>\n";

                source += '\n';

                if (input.Output.FunctionDeclarations != "")
                {
                    source += input.Output.FunctionDeclarations;
                    source += '\n';
                }

                if (input.Output.FunctionDefinitions != "")
                    source += input.Output.FunctionDefinitions.Substring(0, input.Output.FunctionDefinitions.Length - 2);

                // Write files
                File.WriteAllText($"{outputPath}.hpp", header);
                File.WriteAllText($"{outputPath}.cpp", source);
            }
        }
    }
}