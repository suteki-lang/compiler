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
        // Loop through every diagnostic and write it to console
        foreach (Diagnostic diagnostic in Config.Diagnostics)
        {
            string       path     = diagnostic.Path;
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
                    diagnosticColor = ConsoleColor.Blue;
                    break;
                }
            }

            // Write location information
            Console.Error.WriteColor(ConsoleColor.White, 
                $"{path}:{location.Line}:{location.Column}: ");
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
        }

        // Get pass color
        ConsoleColor passedColor = (Config.HasFatalErrors) ? ConsoleColor.Red
                                                           : ConsoleColor.Green;

        // Write a little message to the console
        string passed = (Config.HasFatalErrors) ? "failure"
                                                : "successful";

        if (Config.Diagnostics.Count > 0)
            Console.Error.WriteColorLine(passedColor, $"\nBuild {passed} with {Config.Diagnostics.Count} diagnostic(s).");
        else
            Console.Error.WriteColorLine(passedColor, $"Build {passed} with no diagnostics.");
    }
}