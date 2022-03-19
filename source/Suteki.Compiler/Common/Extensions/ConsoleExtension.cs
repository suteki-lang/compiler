namespace Suteki.Compiler;

using System.IO;
using System;

/// <summary>
/// A class to write colored content to console.
/// </summary>
public static class ConsoleExtension
{
    /// <summary>
    /// Write content with color to console.
    /// </summary>
    /// <param name="stream">The console stream to write.</param>
    /// <param name="content">The content to write to console.</param>
    public static void WriteColor(this TextWriter stream, params object[] content)
    {
        foreach (object value in content)
        {
            if (value.GetType() == typeof(ConsoleColor))
                Console.ForegroundColor = ((ConsoleColor)value);
            else
                stream.Write(value);
        }

        Console.ResetColor();
    }

    /// <summary>
    /// Write content with color with line at end to console.
    /// </summary>
    /// <param name="stream">The console stream to write.</param>
    /// <param name="content">The content to write to console.</param>
    public static void WriteColorLine(this TextWriter stream, params object[] content)
    {
        stream.WriteColor(content);
        stream.WriteLine();
    }
}