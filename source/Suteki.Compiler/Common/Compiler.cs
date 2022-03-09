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
                
                // Get diagnostic color
                ConsoleColor diagnosticColor = ConsoleColor.Red; // for error diagnostic

                switch (diagnostic.Kind)
                {
                    // Warning
                    case DiagnosticKind.Warning:
                    {
                        diagnosticColor = ConsoleColor.Yellow;
                        break;
                    }

                    // Note
                    case DiagnosticKind.Note:
                    {
                        diagnosticColor = ConsoleColor.Yellow;
                        break;
                    }
                }

                // Write location information
                Console.Error.Write(ConsoleColor.White, 
                    $"{input.Path}:{location.Line}:{location.Column}: ");
                Console.Error.Write(diagnosticColor, 
                    $"{diagnostic.Kind.ToString().ToLower()}");
                Console.Error.WriteLine(ConsoleColor.White, $": {diagnostic.Content}");

                // Get location content without spacing at start
                int index;
                for (index = 0; content[index] == ' '; ++index);

                string newContent = content.Substring(index, (content.Length - index));

                // Get line information
                string lineInformation = $"{location.Line} ";
                
                // Write "...|"
                for (int i = 0; i < lineInformation.Length; ++i)
                    Console.Error.Write(ConsoleColor.White, ' ');

                Console.Error.WriteLine(ConsoleColor.White, "|");

                // Write "<line> | <content>"
                Console.Error.Write    (diagnosticColor,    $"{lineInformation}");
                Console.Error.WriteLine(ConsoleColor.White, $"| {newContent}");

                // Write "...| "
                for (int i = 0; i < lineInformation.Length; ++i)
                    Console.Error.Write(ConsoleColor.White, ' ');

                Console.Error.Write(ConsoleColor.White, "| ");

                // Get start position
                int startPosition = (location.Column > index)                       ?
                                    (location.Column - index - location.Length - 1) :
                                    0;
                                    
                // Write "^~~..."
                for (int i = 0; i < startPosition; ++i)
                    Console.Error.Write(diagnosticColor, ' ');

                Console.Error.Write(diagnosticColor, '^');

                for (int i = 1; i < location.Length; ++i)
                    Console.Error.Write(diagnosticColor, '~');

                Console.Error.WriteLine();

                ++diagnoticCount;
            }
        }

        // Get pass color
        ConsoleColor passedColor = (Config.HasFatalErrors) ? ConsoleColor.Red
                                                           : ConsoleColor.Green;

        // Write a little message to the console
        string passed = (Config.HasFatalErrors) ? "failure"
                                                : "successful";

        if (diagnoticCount > 0)
            Console.Error.WriteLine(passedColor, $"\nBuild {passed} with {diagnoticCount} diagnostic(s).");
        else
            Console.Error.WriteLine(passedColor, $"Build {passed} with no diagnostics.");
    }
}