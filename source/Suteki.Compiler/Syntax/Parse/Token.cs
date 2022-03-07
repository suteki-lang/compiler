namespace Suteki.Compiler;

/// <summary>
/// A structure that represents a <see cref="Scanner"/>'s token.
/// </summary>
public sealed class Token
{
    /// <summary>
    /// The kind of the <see cref="Token"/>.
    /// </summary>
    public TokenKind Kind;

    /// <summary>
    /// The content of the <see cref="Token"/>.
    /// </summary>
    public string Content;

    /// <summary>
    /// The location of the <see cref="Token"/> in a file.
    /// </summary>
    public FileLocation Location;

    /// <summary>
    /// Constructs a <see cref="Token"/> class.
    /// </summary>
    /// <param name="kind"    >The kind of the token.              </param>
    /// <param name="content" >The content of the token.           </param>
    /// <param name="location">The location of the token in a file.</param>
    public Token(TokenKind kind, string content, FileLocation location)
    {
        Kind     = kind;
        Content  = content;
        Location = location;
    }
}