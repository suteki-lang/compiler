namespace Suteki.Compiler;

using System.IO;

/// <summary>
/// The class of the program.
/// </summary>
public static class Program
{   
    /// <summary>
    /// Reads file and adds <see cref="Input"/>.
    /// </summary>
    /// <param name="path">The file path to read.</param>
    private static void ReadFile(string path)
    {
        // Read file
        string content = File.ReadAllText(path);

        // Replace all spacing with single space
        content = content.Replace('\t', ' ');
        content = content.Replace('\v', ' ');
        content = content.Replace('\r', ' ');

        // Add a null terminator (so it's "faster" to scan)
        content = (content + '\0');

        // Add input
        Config.Inputs.Add(new Input(path, content));
    }

    /// <summary>
    /// Reads all files from directories and add <see cref="Input"/>s.
    /// </summary>
    /// <param name="path">The path of the directory or file.</param>
    /// <returns><see langword="true"/> if everything was read correctly.</returns>
    private static bool AddInputs(string path)
    {
        // Check if file
        if (path.EndsWith(".su") && File.Exists(path))
        {
            ReadFile(path);
            return true;
        }
        else if (Directory.Exists(path))
        {
            // Read files from directory
            foreach (string file in Directory.GetFiles(path))
            {
                // Only read the file if the extension is .su
                if (path.EndsWith(".su"))
                    ReadFile(path);
            }

            // Read files from directories
            foreach (string directory in Directory.GetDirectories(path))
            {
                if (!AddInputs(directory))
                    return false;
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Parses command line arguments.
    /// </summary>
    /// <param name="arguments">The command line arguments.</param>
    /// <returns><see langword="true"/> if everything was parsed correctly.</returns>
    private static bool ParseArguments(string[] arguments)
    {
        // Loop through each argument and parse it
        for (int index = 0; index < arguments.Length; ++index)
        {
            string argument = arguments[index];

            // Check for option
            if (argument.StartsWith('-'))
            {
                // TODO: handle options
            }
            else
            {
                // This is probably an input.
                if (!AddInputs(argument))
                {
                    // TODO: handle error
                }
            }
        }

        // Check for inputs
        if (Config.Inputs.Count == 0)
        {
            // TODO: handle error
            return false;
        }

        return true;
    }

    /// <summary>
    /// The entry point of the program.
    /// </summary>
    /// <param name="arguments">The command line arguments.</param>
    public static void Main(string[] arguments)
    {
        ParseArguments(arguments);

        Parser p = new Parser();
        p.Start();

        Compiler c = new Compiler();
        c.WriteDiagnostics();
    }
}