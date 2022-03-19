namespace Suteki.Compiler;

using System;

/// <summary>
/// Compiles source files.
/// </summary>
public sealed class Compiler
{
    /// <summary>
    /// Write <see cref="Diagnostic"/>s to the console.
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
                Console.Error.WriteColor(ConsoleColor.White, 
                    $"{input.Path}:{location.Line}:{location.Column}: ");
                Console.Error.WriteColor(diagnosticColor, 
                    $"{diagnostic.Kind.ToString().ToLower()}");
                Console.Error.WriteColorLine(ConsoleColor.White, $": {diagnostic.Content}");

                // Get location content without spacing at start
                int index;
                for (index = 0; content[index] == ' '; ++index);

                string newContent = content.Substring(index, (content.Length - index));

                // Get line information
                string lineInformation = $"{location.Line} ";
                
                // Write "...|"
                for (int i = 0; i < lineInformation.Length; ++i)
                    Console.Error.WriteColor(ConsoleColor.White, ' ');

                Console.Error.WriteColorLine(ConsoleColor.White, "|");

                // Write "<line> | <content>"
                Console.Error.WriteColor    (diagnosticColor,    $"{lineInformation}");
                Console.Error.WriteColorLine(ConsoleColor.White, $"| {newContent}");

                // Write "...| "
                for (int i = 0; i < lineInformation.Length; ++i)
                    Console.Error.WriteColor(ConsoleColor.White, ' ');

                Console.Error.WriteColor(ConsoleColor.White, "| ");

                // Get start position
                int startPosition = (location.Column > index)                       ?
                                    (location.Column - index - location.Length - 1) :
                                    0;
                                    
                // Write "^~~..."
                for (int i = 0; i < startPosition; ++i)
                    Console.Error.WriteColor(diagnosticColor, ' ');

                Console.Error.WriteColor(diagnosticColor, '^');

                for (int i = 1; i < location.Length; ++i)
                    Console.Error.WriteColor(diagnosticColor, '~');

                Console.Error.WriteColorLine();

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
            Console.Error.WriteColorLine(passedColor, $"\nBuild {passed} with {diagnoticCount} diagnostic(s).");
        else
            Console.Error.WriteColorLine(passedColor, $"Build {passed} with no diagnostics.");
    }
}
