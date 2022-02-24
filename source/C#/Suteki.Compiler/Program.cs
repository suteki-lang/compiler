namespace Suteki.Compiler;

using Suteki.Utilities;
using System.IO;
using System;

class Program
{
    // Add all inputs from path
    public static bool AddInputs(string path)
    {
        // Add inputs from directory
        if (Directory.Exists(path))
        {
            // Get all files and directories
            string[] files       = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);

            // Add inputs
            foreach (string file in files)
                Config.Inputs.Add(new Input(file, File.ReadAllText(file) + '\0'));

            // Add inputs from directories
            foreach (string directory in directories)
                AddInputs(directory);
        }

        // Add input
        else if (path.EndsWith(".su") && File.Exists(path))
            Config.Inputs.Add(new Input(path, File.ReadAllText(path) + '\0'));

        // Invalid input
        else
            return false;

        return true;
    }

    // Parse command line
    public static bool ParseCommandLine(string[] arguments)
    {
        for (uint index = 0; index < arguments.Length; ++index)
        {
            string argument = arguments[index];

            // Input(s)?
            if (!argument.StartsWith('-'))
            {
                if (!AddInputs(argument))
                {
                    Console.Error.WriteLine(ConsoleColor.Red, "Error: ", ConsoleColor.White, $"{argument}: ", "No such file or directory.");
                    return false;
                }
            }
            else
            {
                Console.Error.WriteLine(ConsoleColor.Red, "Error: ", ConsoleColor.White, "No input files.");
                return false;
            }
        }

        // Check for output path
        if (Config.OutputPath == "")
        {
            if (!Directory.Exists("output"))
                Directory.CreateDirectory("output");

            Config.OutputPath = "output/";
        }

        // Check for runtime path
        if (Config.RuntimePath == "")
            Config.RuntimePath = "../../../runtime/";

        // Check for input files
        if (Config.Inputs.Count == 0)
        {
            Console.Error.WriteLine(ConsoleColor.Red, "Error: ", ConsoleColor.White, "No input files.");
            return false;
        }

        return true;
    }

    public static void Main(string[] arguments)
    {   
        // Parse the command line options
        if (!ParseCommandLine(arguments))
            return;

        // Compile
        Parser parser = new Parser();
        parser.Start();

        if (!Config.HadError)
        {
            Emitter.Start();
            Linker.Start();
        }
    }
}