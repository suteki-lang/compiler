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
    Bool,
    Null,

    // Single Character
    LeftParenthesis,
    RightParenthesis,
    LeftBrace,
    RightBrace,
    Comma,
    Semicolon,
    Dot,
    Plus,
    Minus,
    Slash,
    Star,

    // Multiple Characters
    Arrow,
    EqualEqual,

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