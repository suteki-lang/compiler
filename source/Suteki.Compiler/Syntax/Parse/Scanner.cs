namespace Suteki.Compiler;

using System.Collections.Generic;

/// <summary>
/// Scans tokens from source file.
/// </summary>
public sealed class Scanner
{
    /// <summary>
    /// All keywords of the language.
    /// </summary>
    public Dictionary<string, TokenKind> Keywords = new Dictionary<string, TokenKind>()
    {
        { "true",    TokenKind.Bool    },
        { "false",   TokenKind.Bool    },
        { "null",    TokenKind.Null    },

        { "module",  TokenKind.Module  },
        { "import",  TokenKind.Import  },

        { "extern",  TokenKind.Extern  },
        { "public",  TokenKind.Public  },
        { "private", TokenKind.Private },

        { "const",   TokenKind.Const   },

        { "if",      TokenKind.If      },
        { "else",    TokenKind.Else    },

        { "return", TokenKind.Return  },
    };

    /// <summary>
    /// The source being scanned.
    /// </summary>
    public string Source;

    /// <summary>
    /// The current line.
    /// </summary>
    public int Line;

    /// <summary>
    /// The current column of the line.
    /// </summary>
    public int Column;

    /// <summary>
    /// The start of the <see cref="Token"/>'s content.
    /// </summary>
    public int Start;

    /// <summary>
    /// The end of the <see cref="Token"/>'s content.
    /// </summary>
    public int End;

    /// <summary>
    /// The start of the line.
    /// </summary>
    public int LineStart;

    /// <summary>
    /// The previously <see cref="Token"/> that was scanned.
    /// </summary>
    public Token Previous;

    /// <summary>
    /// The last <see cref="Token"/> that was scanned.
    /// </summary>
    public Token Current;

    /// <summary>
    /// Constructs a <see cref="Scanner"/> class.
    /// </summary>
    /// <param name="source">The source to be scanned.</param>
    public Scanner(string source)
    {
        Source    = source;
        Line      = 1;
        Column    = 1;
        Start     = 0;
        End       = 0;
        LineStart = 0;
        Previous  = null;
        Current   = null;
    }

    /// <summary>
    /// The current location of the <see cref="Token"/>.
    /// </summary>
    private FileLocation CurrentLocation
    {
        get
        {
            // Find next newline or EOF
            int end = LineStart;

            while (Source[end] != '\0' && Source[end] != '\n')
                ++end;

            // Replace all spacing with spaces
            string content = Source.Substring(LineStart, (end - LineStart));
            return new FileLocation(Line, Column, content, (End - Start));
        }
    }

    /// <summary>
    /// Checks if character is a letter or underscore.
    /// </summary>
    /// <param name="character">The character to be checked.</param>
    private bool IsLetterOrUnderscoe(char character)
    {
        return (character >= 'A' && character <= 'Z') ||
               (character >= 'a' && character <= 'z') ||
               (character == '_');
    }

    /// <summary>
    /// Checks if character is a digit.
    /// </summary>
    /// <param name="character">The character to be checked.</param>
    private bool IsDigit(char character)
    {
        return (character >= '0' && character <= '9');
    }

    /// <summary>
    /// Advance the current character.
    /// </summary>
    /// <returns>The previous character that was advanced.</returns>
    private char Advance()
    {
        ++Column;
        ++End;

        return Source[End - 1];
    }

    /// <summary>
    /// Checks if character matches the current character.
    /// </summary>
    /// <param name="expected">The character to be checked.</param>
    private bool Match(char expected)
    {
        if (Source[End] == expected)
        {
            Advance();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Skips all whitespaces.
    /// </summary>
    private void SkipWhitespace()
    {
        for (;;)
        {
            switch (Source[End])
            {
                // Line
                case '\n':
                {
                    Line      += 1;
                    Column     = 0;
                    LineStart  = (End + 1);

                    Advance();
                    break;
                }

                // Spacing
                case ' ':
                {
                    Advance();
                    break;
                }

                // Comment?
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

    /// <summary>
    /// Make <see cref="Token"/>.
    /// </summary>
    /// <param name="kind">The <see cref="TokenKind"/> of the <see cref="Token"/>.</param>
    /// <returns>The <see cref="TokenKind"/> of the <see cref="Token"/>.</returns>
    private TokenKind MakeToken(TokenKind kind)
    {
        Current = new Token(kind, Source.Substring(Start, (End - Start)), CurrentLocation);
        return kind;
    }

    /// <summary>
    /// Make <see cref="Token"/> with content.
    /// </summary>
    /// <param name="kind"   >The <see cref="TokenKind"/> of the <see cref="Token"/>.</param>
    /// <param name="content">The content of the <see cref="Token"/>.                </param>
    /// <returns>The <see cref="TokenKind"/> of the <see cref="Token"/>.</returns>
    private TokenKind MakeToken(TokenKind kind, string content)
    {
        Current = new Token(kind, content, CurrentLocation);
        return kind;
    }

    /// <summary>
    /// Make error <see cref="Token"/>.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns><see cref="TokenKind.Error"/></returns>
    private TokenKind MakeToken(string message)
    {
        Current = new Token(TokenKind.Error, message, CurrentLocation);
        return TokenKind.Error;
    }

    /// <summary>
    /// Make string <see cref="Token"/>.
    /// </summary>
    /// <returns><see cref="TokenKind.String"/> if string was parsed or <see cref="TokenKind.Error"/> if it failed.</returns>
    private TokenKind MakeStringToken()
    {
        while (Source[End] != '"' && Source[End] != '\0')
            Advance();

        if (Source[End] == '\0')
            return MakeToken("Unterminated string.");

        Advance();
        return MakeToken(TokenKind.String);
    }
    /// <summary>
    /// Make integer or float <see cref="Token"/>.
    /// </summary>
    /// <returns><see cref="TokenKind.Float"/> if the parsed number is a float or <see cref="TokenKind.Integer"/> if the parsed number is an integer.</returns>
    private TokenKind MakeNumberToken()
    {
        while (IsDigit(Source[End]))
            Advance();

        // Float?
        if (Source[End] == '.')
        {
            Advance();

            while (IsDigit(Source[End]))
                Advance();

            return MakeToken(TokenKind.Float);
        }

        return MakeToken(TokenKind.Integer);
    }

    /// <summary>
    /// Make identifier or keyword <see cref="Token"/>.
    /// </summary>
    /// <returns><see cref="TokenKind.Identifier"/> if is an identifier else keyword <see cref="TokenKind"/>.</returns>
    private TokenKind MakeIdentifierToken()
    {
        while (IsLetterOrUnderscoe(Source[End]) || IsDigit(Source[End]))
            Advance();

        // Is a keyword?
        string identifier = Source.Substring(Start, (End - Start));

        if (Keywords.ContainsKey(identifier))
            return MakeToken(Keywords[identifier], identifier);

        return MakeToken(TokenKind.Identifier, identifier);
    }

    /// <summary>
    /// Scan the next <see cref="Token"/>.
    /// </summary>
    /// <returns>The <see cref="TokenKind"/> of the scanned <see cref="Token"/>.</returns>
    public TokenKind Next()
    {
        SkipWhitespace();

        Start    = End;
        Previous = Current;

        // Is source end?
        if (Source[End] == '\0')
            return MakeToken(TokenKind.End);

        // Advance current character
        char character = Advance();

        // Try scanning multiple character 
        if (IsDigit(character))
            return MakeNumberToken();

        if (IsLetterOrUnderscoe(character))
            return MakeIdentifierToken();

        if (character == '"')
            return MakeStringToken();

        // Try scanning single character tokens
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

            case '+':
                return MakeToken(TokenKind.Plus);
                
            case '-':
                return MakeToken(TokenKind.Minus);
                
            case '/':
                return MakeToken(TokenKind.Slash);
                
            case '*':
                return MakeToken(TokenKind.Star);

            case '=':
            {
                // =>
                if (Match('>'))
                    return MakeToken(TokenKind.Arrow);

                // ==
                if (Match('='))
                    return MakeToken(TokenKind.EqualEqual);

                break;
            }
        }

        return MakeToken("unexpected character.");
    }
}