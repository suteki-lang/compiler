namespace Suteki.Compiler;

public enum TokenKind
{
    Error,
    End,

    Identifier,
    Integer,
    Float,
    String,
    Bool,
    Null,

    LeftParenthesis,
    RightParenthesis,
    LeftBrace,
    RightBrace,
    Comma,
    Semicolon,
    Dot,
    Star,
    Plus,
    Minus,
    Slash,
    Equal,
    EqualEqual,
    Arrow,

    Module,
    Import,

    Public,
    Private,
    
    Extern,

    Const,

    If,
    Else,

    Return,
}

public class Token
{
    public TokenKind Kind;
    public string    Content;
    public int       Line;
    public int       Column;

    // Create token with same line information and kind but different content
    public static Token From(Token previous, string content)
    {
        return new Token()
        {
            Kind    = previous.Kind,
            Content = content,
            Line    = previous.Line,
            Column  = previous.Column
        };
    }

    // Convert TokenKind to OperatorKind
    public static OperatorKind ToOperatorKind(TokenKind kind)
    {
        switch (kind)
        {
            case TokenKind.Plus:
                return OperatorKind.Add;

            case TokenKind.Minus:
                return OperatorKind.Subtract;

            case TokenKind.Slash:
                return OperatorKind.Divide;

            case TokenKind.Star:
                return OperatorKind.Multiply;

            case TokenKind.EqualEqual:
                return OperatorKind.Equality;
            
            default:
                return OperatorKind.None;
        }
    }
}