using System;

namespace Suteki
{
    class Logger
    {
        public string Path     = "";

        // Show error
        public void Error(string message)
        {
            Config.HadError = true;
            Console.Error.WriteLine($"[{Path}] Error: {message}");
        }

        // Show error at token
        public void Error(Token token, string message)
        {
            Config.HadError = true;
            Console.Error.Write($"[{Path}:{token.Line}:{token.Column}] Error");

            switch (token.Kind)
            {
                case TokenKind.End:
                {
                    Console.Error.Write(" at end");
                    break;
                }

                case TokenKind.Error:
                {
                    Console.Error.Write(": ");
                    break;
                }

                default:
                {
                    Console.Error.Write($" at '{token.Data.ToString()}': ");
                    break;
                }
            }

            Console.Error.WriteLine(message);
        }

        // Show fatal error
        public void FatalError(string message)
        {
            Console.Error.WriteLine($"[{Path}] Fatal Error: {message}");
            Environment.Exit(1);
        }

        // Show fatal error at token
        public void FatalError(Token token, string message)
        {
            Console.Error.Write($"[{Path}:{token.Line}:{token.Column}] Fatal Error");

            switch (token.Kind)
            {
                case TokenKind.End:
                {
                    Console.Error.Write(" at end");
                    break;
                }

                case TokenKind.Error:
                {
                    Console.Error.Write(": ");
                    break;
                }

                default:
                {
                    Console.Error.Write($" at '{token.Data.ToString()}': ");
                    break;
                }
            }

            Console.Error.WriteLine(message);
            Environment.Exit(1);
        }
    }
}