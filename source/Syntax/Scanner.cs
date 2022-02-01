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
            { "module", TokenKind.Module },
            { "import", TokenKind.Import },

            { "extern", TokenKind.Extern },

            { "const", TokenKind.Const   },

            { "true",  TokenKind.Bool    },
            { "false", TokenKind.Bool    },
            { "null",  TokenKind.Null    },

            { "return", TokenKind.Return },
        };

        // Set scanner input
        public void Set(string source)
        {
            Line     = 1;
            Column   = 1;
            Start    = 0;
            End      = 0;
            Source   = source;
            Previous = null;
            Current  = null;
        }

        // Advance character
        private char Advance()
        {
            Column += 1;
            End     = System.Math.Clamp(End, End + 1, Source.Length);

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
        private TokenKind SkipWhitespace()
        {
            for (;;)
            {
                // Is source end?
                if (End >= Source.Length)
                    return TokenKind.End;

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

                    case '/':
                    {
                        if ((End + 1) >= Source.Length)
                            break;

                        // Single line comment
                        if (Source[End + 1] == '/')
                        {
                            while (End < Source.Length && Source[End] != '\n')
                                Advance();

                            Line   += 1;
                            Column  = 0;
                        }

                        break;
                    }

                    default:
                        return TokenKind.End;
                }
            }
        }

        // Make token
        private TokenKind MakeToken(TokenKind kind)
        {
            Current = new Token
            { 
                Kind    = kind,
                Content = Source.Substring(Start, (End - Start)),
                Line    = Line,
                Column  = Column,
            };

            return kind;
        }

        // Make token
        private TokenKind MakeToken(TokenKind kind, string content)
        {
            Current = new Token
            { 
                Kind    = kind,
                Content = content,
                Line    = Line,
                Column  = Column,
            };

            return kind;
        }

        // Make error token
        private TokenKind MakeToken(string message)
        {
            Current = new Token
            { 
                Kind    = TokenKind.Error,
                Content = message,
                Line    = Line,
                Column  = Column,
            };

            return TokenKind.Error;
        }

        // Make number token
        private TokenKind MakeNumberToken()
        {
            while (End < Source.Length && IsNumber(Source[End]))
                Advance();

            if (Match('.'))
            {
                while (End < Source.Length && IsNumber(Source[End]))
                    Advance();

                return MakeToken(TokenKind.Float);
            }

            return MakeToken(TokenKind.Integer);
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
            TokenKind token = SkipWhitespace();

            if (token != TokenKind.End)
                return TokenKind.Error;

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

                case '*':
                    return MakeToken(TokenKind.Star);

                case '"':
                    return MakeStringToken();

                default:
                    return MakeToken("Unexpected character.");
            }
        }
    }
}