namespace Suteki.Compiler;

/// <summary>
/// A node that represents a string.
/// </summary>
public class NodeString : Node
{
    /// <summary>
    /// The token of the string.
    /// </summary>
    public Token Value;

    /// <summary>
    /// Constructs a <see cref="NodeString"/> class.
    /// </summary>
    /// <param name="location">The location of the node.</param>
    /// <param name="value"   >The string token.        </param>
    public NodeString(FileLocation location, Token value)
    {
        Location = location;
        Value    = value;
    }

    /// <summary>
    /// Accepts the visitor.
    /// </summary>
    public override T AcceptVisitor<T>(ASTVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}