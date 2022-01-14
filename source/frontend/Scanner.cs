using System;
using System.Collections.Generic;

namespace Suteki
{
    class Scanner
    {
        private int    Line;
        private int    Column;
        private int    Start;
        private int    End;
        private string Source;
        public  Token  Previous;
        public  Token  Current;

        private Dictionary<string, TokenKind> Keywords = new Dictionary<string, TokenKind>
        {
            { "export", TokenKind.Export },
            { "import", TokenKind.Import },

            { "extern", TokenKind.Extern },

            { "true",  TokenKind.Bool    },
            { "false", TokenKind.Bool    },

            { "return", TokenKind.Return },
        };

        // Set scanner input
        public void Set(string source)
        {
            Line     = 0;
            Column   = 0;
            Start    = 0;
            End      = 0;
            Source   = source;
            Previous = null;
            Current  = null;
        }

        // Advance character
        private char Advance()
        {
            ++End;
            ++Column;

            return Source[End - 1];
        }

        // Match current character?
        private bool Match(char expected)
        {
            if (End < Source.Length && Source[End] == expected)
            {
                Advance();
                return true;
            }

            return false;
        }

        // Character matches identifier?
        private bool IsIdentifier(char character)
        {
            return (character >= 'A' && character <= 'Z') ||
                   (character >= 'a' && character <= 'z') ||
                   (character == '_');
        }

        // Character matches number?
        private bool IsNumber(char character)
        {
            return (character >= '0' && character <= '9');
        }

        // Skip whitespace
        private void SkipWhitespace()
        {
            for (;;)
            {
                // Is source end?
                if (End >= Source.Length)
                    return;

                switch (Source[End])
                {
                    case '\n':
                    {
                        Advance();

                        Line   += 1;
                        Column  = 0;

                        break;
                    }

                    case ' ' :
                    case '\r':
                    case '\t':
                    {
                        Advance();
                        break;
                    }

                    default:
                        return;
                }
            }
        }

        // Make token
        private TokenKind MakeToken(TokenKind kind)
        {
            Current = new Token
            { 
                Kind   = kind,
                Data   = Source.Substring(Start, (End - Start)),
                Line   = Line,
                Column = Column,
            };

            return kind;
        }

        // Make token
        private TokenKind MakeToken(TokenKind kind, object data)
        {
            Current = new Token
            { 
                Kind   = kind,
                Data   = data,
                Line   = Line,
                Column = Column,
            };

            return kind;
        }

        // Make error token
        private TokenKind MakeToken(string message)
        {
            Current = new Token
            { 
                Kind   = TokenKind.Error,
                Data   = message,
                Line   = Line,
                Column = Column,
            };

            return TokenKind.Error;
        }

        // Make number token
        private TokenKind MakeNumberToken()
        {
            bool isFloat = false;

            while (End < Source.Length && IsNumber(Source[End]))
                Advance();

            if (Match('.'))
            {
                isFloat = true;

                while (End < Source.Length && IsNumber(Source[End]))
                    Advance();
            }

            string numberString = Source.Substring(Start, (End - Start));
            return MakeToken((isFloat) ? TokenKind.Float : TokenKind.Integer, double.Parse(numberString));
        }

        // Make identifier token
        private TokenKind MakeIdentifierToken()
        {
            while (End < Source.Length && IsIdentifier(Source[End]))
                Advance();

            string identifierString = Source.Substring(Start, (End - Start));

            if (Keywords.ContainsKey(identifierString))
                return MakeToken(Keywords[identifierString]);

            return MakeToken(TokenKind.Identifier, identifierString);
        }

        // Make string token
        private TokenKind MakeStringToken()
        {
            while (End < Source.Length && Source[End] != '"')
                Advance();

            if (End >= Source.Length)
                return MakeToken("Unterminated string.");

            Advance();
            return MakeToken(TokenKind.String);
        }

        // Scan token
        public TokenKind Next()
        {
            SkipWhitespace();

            Previous = Current;
            Start    = End;

            // Is source end?
            if (End >= Source.Length)
                return MakeToken(TokenKind.End, "<EOF>");

            char character = Advance();

            if (IsNumber(character))
                return MakeNumberToken();

            if (IsIdentifier(character))
                return MakeIdentifierToken();

            switch (character)
            {
                case '(':
                    return MakeToken(TokenKind.LeftParenthesis);

                case ')':
                    return MakeToken(TokenKind.RightParenthesis);

                case '{':
                    return MakeToken(TokenKind.LeftBrace);

                case '}':
                    return MakeToken(TokenKind.RightBrace);

                case ',':
                    return MakeToken(TokenKind.Comma);

                case ';':
                    return MakeToken(TokenKind.Semicolon);

                case '.':
                    return MakeToken(TokenKind.Dot);

                case '"':
                    return MakeStringToken();

                default:
                    return MakeToken("Unexpected character.");
            }
        }
    }
}