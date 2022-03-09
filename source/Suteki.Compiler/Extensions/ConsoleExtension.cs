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
    /// <param name="color">The color of the content.</param>
    /// <param name="content">The content to write to console.</param>
    public static void Write(this TextWriter stream, ConsoleColor color, dynamic content)
    {
        Console.ForegroundColor = color;
        stream.Write(content);
        Console.ResetColor();
    }

    /// <summary>
    /// Write content with color with line at end to console.
    /// </summary>
    /// <param name="stream">The console stream to write.</param>
    /// <param name="color">The color of the content.</param>
    /// <param name="content">The content to write to console.</param>
    public static void WriteLine(this TextWriter stream, ConsoleColor color, dynamic content)
    {
        Console.ForegroundColor = color;
        stream.WriteLine(content);
        Console.ResetColor();
    }
}