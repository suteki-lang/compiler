namespace Suteki.Compiler;

using System;

/// <summary>
/// Compiles source files.
/// </summary>
public sealed class Compiler
{
    /// <summary>
    /// Write <see cref="Diagnotic"/>s to the console.
    /// </summary>
    public void WriteDiagnostics()
    {
        // A variable to know the amount of diagnotics written to console.
        int diagnoticCount = 0;

        // Loop through every input and write the diagnostics to console
        foreach (Input input in Config.Inputs)
        {
            // No diagnostics?
            if (input.Diagnostics.Count == 0)
                break;

            // Loop through every diagnostic and write it to console
            foreach (Diagnostic diagnostic in input.Diagnostics)
            {
                FileLocation location = diagnostic.Location;
                string       content  = location.Content;

                // Write location information
                Console.Error.Write($"{input.Path}:{location.Line}:{location.Column}: ");
                Console.Error.WriteLine($"{diagnostic.Kind.ToString().ToLower()}: {diagnostic.Content}");

                // Get location content without spacing at start
                int index;
                for (index = 0; content[index] == ' '; ++index);

                string newContent = content.Substring(index, (content.Length - index));

                // Write line
                string lineInformation = $"{location.Line} |";
                

                for (int i = 1; i < lineInformation.Length; ++i)
                    System.Console.Write(' ');

                System.Console.WriteLine("|");

                System.Console.WriteLine($"{lineInformation} {newContent}");

                // Write location
                for (int i = 1; i < lineInformation.Length; ++i)
                    System.Console.Write(' ');

                System.Console.Write("| ");

                int startPosition = (location.Column > index)                       ?
                                    (location.Column - index - location.Length - 1) :
                                    0;

                for (int i = 0; i < startPosition; ++i)
                    System.Console.Write(' ');

                System.Console.Write('^');

                for (int i = 1; i < location.Length; ++i)
                    System.Console.Write('~');

                System.Console.WriteLine();

                ++diagnoticCount;
            }
        }

        // Write a little message to the console
        string passed = (Config.HasFatalErrors) ? "failure"
                                                : "successful";

        if (diagnoticCount > 0)
            Console.Error.WriteLine($"\nBuild {passed} with {diagnoticCount} diagnostic(s).");
        else
            Console.Error.WriteLine($"Build {passed} with no diagnostics.");
    }
}