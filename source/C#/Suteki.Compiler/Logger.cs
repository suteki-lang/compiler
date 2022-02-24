namespace Suteki.Compiler;

using Suteki.Utilities;
using System;

public class Logger
{
    public string Path;

    // Show error
    public void Error(string message)
    {
        Config.HadError = true;
        
        if (!Config.CanLog)
            return;

        Console.Error.WriteLine(ConsoleColor.Red, $"[{Path}] Error: ", ConsoleColor.White, message);
    }

    // Show error at token
    public void Error(Token token, string message)
    {
        Config.HadError = true;

        if (!Config.CanLog)
            return;

        Console.Error.Write(ConsoleColor.Red, $"[{Path}:{token.Line}:{token.Column}] Error");

        switch (token.Kind)
        {
            case TokenKind.End:
            {
                Console.Error.Write(ConsoleColor.Red, " at end: ");
                break;
            }

            case TokenKind.Error:
            {
                Console.Error.Write(ConsoleColor.Red, ": ");
                break;
            }

            default:
            {
                Console.Error.Write(ConsoleColor.Red,    " at ");
                Console.Error.Write(ConsoleColor.White, $"'{token.Content}'");
                Console.Error.Write(ConsoleColor.Red,    ": ");
                break;
            }
        }

        Console.Error.WriteLine(ConsoleColor.White, message);
    }
}