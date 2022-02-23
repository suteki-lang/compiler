namespace Suteki.Compiler;

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

        Console.Error.WriteLine($"[{Path}] Error: {message}");
    }

    // Show error at token
    public void Error(Token token, string message)
    {
        Config.HadError = true;

        if (!Config.CanLog)
            return;

        Console.Error.Write($"[{Path}:{token.Line}:{token.Column}] Error");

        switch (token.Kind)
        {
            case TokenKind.End:
            {
                Console.Error.Write(" at end: ");
                break;
            }

            case TokenKind.Error:
            {
                Console.Error.Write(": ");
                break;
            }

            default:
            {
                Console.Error.Write($" at '{token.Content}': ");
                break;
            }
        }

        Console.Error.WriteLine(message);
    }
}