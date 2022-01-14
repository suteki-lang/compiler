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

                // Header: Write namespace
                header += "namespace User\n{\n";

                if (input.Output.Header.Length != 0)
                    header += input.Output.Header;
                else
                    header += "\t\n";

                header += "}\n";

                // Header: End guard
                header += "\n#endif";

                // Source: Add includes
                source += $"#include <{path}.hpp>\n";
                source += input.Output.Includes;
                source += "\n";

                // Source: Add namespace
                source += "namespace User\n{\n";

                if (input.Output.Source.Length != 0)
                    source += input.Output.Source;
                else
                    source += "\t\n";

                source += "}\n";

                // Write files
                File.WriteAllText($"{outputPath}.hpp", header);
                File.WriteAllText($"{outputPath}.cpp", source);
            }
        }
    }
}