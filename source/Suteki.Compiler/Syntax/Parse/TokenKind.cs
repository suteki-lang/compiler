namespace Suteki.Compiler;

/// <summary>
/// The kind of a <see cref="Token"/>.
/// </summary>
public enum TokenKind
{
    // Information
    End,
    Error,
    
    // Literals
    Identifier,
    String,
    Integer,
    Float,

    // Single Character
    LeftParenthesis,
    RightParenthesis,
    LeftBrace,
    RightBrace,
    Comma,
    Semicolon,
    Dot,

    // Multiple Characters
    Arrow,

    // Keywords
    Module,
    Import,

    Extern,
    Public,
    Private,

    Const,

    If,
    Else,

    Return,
}