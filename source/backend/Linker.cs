using System.IO;

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
            path = ("user/" + path);

            // Make the directory
            string directory = (Config.OutputPath + path);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return (path + name);            
        }

        // Start linking
        public static void Start()
        {
            // Write files
            foreach (Input input in Config.Inputs)
            {
                string path       = GetPath(input.Path);
                string outputPath = Config.OutputPath + path;
                string guardName  = path.Replace("/", "_").Replace(".", "_").ToUpper();

                string header = "";
                string source = "";

                // Header: Write guard
                header += $"#ifndef {guardName}\n";
                header += $"#define {guardName}\n\n";

                header += input.Output.Header;

                // Header: End guard
                header += "\n#endif";

                // Source: Add includes
                source += $"#include <{path}.hpp>\n";
                source += input.Output.Includes;
                source += "\n";
                source += input.Output.Source;

                // Write files
                File.WriteAllText($"{outputPath}.hpp", header);
                File.WriteAllText($"{outputPath}.cpp", source);
            }
        }
    }
}