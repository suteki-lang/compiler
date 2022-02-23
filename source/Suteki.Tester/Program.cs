namespace Suteki.Tester;

using Suteki.Compiler;
using System.IO;
using System;

public class Program
{
    static void Main()
    {   
        // The directory containing all the directories with files we want to test
        string filesDirectory = "../../tests/files/";

        // Get all directories from the files directory
        string[] directories = Directory.GetDirectories(filesDirectory);

        // Disable logging so it don't show any error message
        Config.CanLog = false;

        // Set the output and runtime path
        Config.OutputPath  = "../../tests/out/";
        Config.RuntimePath = "../../runtime/";

        // Loop through every directory
        foreach (string directory in directories)
        {
            // Get all files from directory
            string[] files = Directory.GetFiles(directory);

            // Compile each file
            foreach (string file in files)
            {
                Console.Write($"Compiling file '{file}'.. ");

                // Add input
                Input input = new Input(file, File.ReadAllText(file) + '\0');
                Config.Inputs.Add(input);

                // Set input module
                input.Module = new Module(file.Replace(filesDirectory, "").Replace(".su", "").Replace("/", "."));
                Config.AddModule(input.Module.Name, input.Module);

                // Parse the input to AST nodes
                Parser parser = new Parser();
                parser.Parse(input);

                if (!Config.HadError)
                    Console.WriteLine("OK");
                else
                    Console.WriteLine("FAILED");
            }
        }

        // Check if had any errors, if not emit and output C++ code.
        if (!Config.HadError)
        {
            Emitter.Start();
            Linker.Start();
        }
    }
}