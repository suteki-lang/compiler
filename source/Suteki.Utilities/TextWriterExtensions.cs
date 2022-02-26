namespace Suteki.Utilities;

using System.IO;
using System;

public static class TextWriterExtensions
{
    public static void Write(this TextWriter stream, params dynamic[] content)
    {
        foreach (dynamic value in content)
        {
            if (value.GetType() == typeof(ConsoleColor))
                Console.ForegroundColor = value;
            else
                stream.Write(value);
        }

        Console.ResetColor();
    }

    public static void WriteLine(this TextWriter stream, params dynamic[] content)
    {
        foreach (dynamic value in content)
        {
            if (value.GetType() == typeof(ConsoleColor))
                Console.ForegroundColor = value;
            else
                stream.Write(value);
        }

        stream.Write('\n');
        Console.ResetColor();
    }
}