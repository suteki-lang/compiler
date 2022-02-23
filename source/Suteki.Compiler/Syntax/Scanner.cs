namespace Suteki.Compiler;

using System.Collections.Generic;

public class Scanner
{
    public int    Line;
    public int    Column;
    public int    Start;
    public int    End;
    public string Source;
    public Token  Previous;
    public Token  Current;

    public Dictionary<string, TokenKind> Keywords = new Dictionary<string, TokenKind>
    {
        { "module",  TokenKind.Module  },
        { "import",  TokenKind.Import  },
        { "public",  TokenKind.Public  },
        { "private", TokenKind.Private },

        { "extern",  TokenKind.Extern  },

        { "const",   TokenKind.Const   },

        { "true",    TokenKind.Bool    },
        { "false",   TokenKind.Bool    },
        { "null",    TokenKind.Null    },

        { "if",      TokenKind.If      },
        { "else",    TokenKind.Else    },

        { "return",  TokenKind.Return  },
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
    public char Advance()
    {
        ++End;
        ++Column;

        return Source[End - 1];
    }

    // Match current character?
    public bool Match(char expected)
    {
        if (Source[End] == expected)
        {
            Advance();
            return true;
        }

        return false;
    }

    // Character matches identifier?
    public bool IsIdentifier(char character)
    {
        return (character >= 'A' && character <= 'Z') ||
               (character >= 'a' && character <= 'z') ||
               (character == '_');
    }

    // Character matches number?
    public bool IsNumber(char character)
    {
        return (character >= '0' && character <= '9');
    }

    // Skip whitespace
    public void SkipWhitespace()
    {
        for (;;)
        {
            switch (Source[End])
            {
                case '\n':
                {
                    Advance();

                    Line   += 1;
                    Column  = 1;
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
                    // Single line comment
                    if (Source[End + 1] == '/')
                    {
                        while (Source[End] != '\n' && Source[End] != '\0')
                            Advance();
                    }
                    else
                    {
                        // It's probably a token.
                        return;
                    }

                    break;
                }

                default:
                    return;
            }
        }
    }

    // Make token
    public TokenKind MakeToken(TokenKind kind)
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
    public TokenKind MakeToken(TokenKind kind, string content)
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
    public TokenKind MakeToken(string message)
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
    public TokenKind MakeNumberToken()
    {
        while (IsNumber(Source[End]))
            Advance();

        if (Match('.'))
        {
            while (IsNumber(Source[End]))
                Advance();

            return MakeToken(TokenKind.Float);
        }

        return MakeToken(TokenKind.Integer);
    }

    // Make identifier token
    public TokenKind MakeIdentifierToken()
    {
        while (IsIdentifier(Source[End]))
            Advance();

        string identifierString = Source.Substring(Start, (End - Start));

        if (Keywords.ContainsKey(identifierString))
            return MakeToken(Keywords[identifierString]);

        return MakeToken(TokenKind.Identifier, identifierString);
    }

    // Make string token
    public TokenKind MakeStringToken()
    {
        while (Source[End] != '"' && Source[End] != '\0')
            Advance();

        if (Source[End] == '\0')
            return MakeToken("Unterminated string.");

        Advance();

        // Replace newlines
        string source = Source.Substring(Start, (End - Start));
               source = source.Replace("\n", "\\n");

        return MakeToken(TokenKind.String, source);
    }

    // Scan token
    public TokenKind Next()
    {
        SkipWhitespace();

        Previous = Current;
        Start    = End;

        // Is source end?
        if (Source[End] == '\0')
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

            case '+':
                return MakeToken(TokenKind.Plus);

            case '-':
                return MakeToken(TokenKind.Minus);

            case '/':
                return MakeToken(TokenKind.Slash);

            case '=':
            {
                if (Match('='))
                    return MakeToken(TokenKind.EqualEqual);

                if (Match('>'))
                    return MakeToken(TokenKind.Arrow);

                return MakeToken(TokenKind.Equal);
            }

            case '"':
                return MakeStringToken();

            default:
                return MakeToken("Unexpected character.");
        }
    }
}